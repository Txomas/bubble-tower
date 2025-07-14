using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Common.Utils
{
    public class AsmdefReferenceResolver
    {
        [Serializable]
        private class Asmdef
        {
            public string[] references;
        }

        private static readonly string[] _ignoredNamespacePrefixes = { ".Editor", ".Tests" };

        [MenuItem(MenuItemPaths.AssetsTools + "Resolve References", false)]
        private static void ResolveReferencesForSelectedAsmdef()
        {
            var selectedAsmdefPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            ResolveReferences(selectedAsmdefPath);
            Debug.Log("References resolved for: " + selectedAsmdefPath);
        }

        [MenuItem(MenuItemPaths.AssetsTools + "Resolve References", true)]
        private static bool ValidateResolveReferencesForSelectedAsmdef()
        {
            var selectedAsmdefPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            return selectedAsmdefPath != null && selectedAsmdefPath.EndsWith(".asmdef");
        }

        [MenuItem(MenuItemPaths.AssetsTools + "Add and Resolve Asmdef", false)]
        private static void AddAndResolveAsmdefForSelectedFolder()
        {
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var folderName = new DirectoryInfo(folderPath).Name;
            var name = $"{folderName}ASM";
            var fileName = $"{name}.asmdef";
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                Debug.LogWarning($"Asmdef file {filePath} already exists.");
            }
            else
            {
                CreateAsmdefFile(filePath);
                Debug.Log($"Asmdef file created: {filePath}");
            }

            ResolveReferences(filePath);
            AssetDatabase.Refresh();
        }

        [MenuItem(MenuItemPaths.AssetsTools + "Add and Resolve Asmdef", true)]
        private static bool ValidateAddAndResolveAsmdefForSelectedFolder()
        {
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            return !string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath);
        }

        private static void CreateAsmdefFile(string path)
        {
            var asmdef = new Asmdef
            {
                references = Array.Empty<string>()
            };
            var asmdefJson = JsonUtility.ToJson(asmdef, true);
            File.WriteAllText(path, asmdefJson);
        }

        private static void ResolveReferences(string asmdefPath)
        {
            var directoryPath = Path.GetDirectoryName(asmdefPath);
            var csFiles = GetCsFilesInDirectory(directoryPath);
            var usingNamespaces = GetUsingNamespacesFromCsFiles(csFiles);
            var allAsmdefGuids = AssetDatabase.FindAssets("t:asmdef");
            var foundReferences = new HashSet<string>();
            
            EditorUtility.DisplayProgressBar("Resolving References", "Analyzing .cs files for using statements...", 0f);

            for (int i = 0; i < allAsmdefGuids.Length; i++)
            {
                var asmdefGuid = allAsmdefGuids[i];
                var otherAsmdefPath = AssetDatabase.GUIDToAssetPath(asmdefGuid);

                if (otherAsmdefPath == asmdefPath) continue;

                var otherAsmdefName = Path.GetFileNameWithoutExtension(otherAsmdefPath);

                if (_ignoredNamespacePrefixes.Any(prefix => otherAsmdefName.Contains(prefix)))
                {
                    continue;
                }

                var progress = (float)i / allAsmdefGuids.Length;
                EditorUtility.DisplayProgressBar("Resolving References", $"Checking namespaces: {otherAsmdefPath}", progress);

                if (DoesAsmdefProvideNamespaces(otherAsmdefPath, usingNamespaces))
                {
                    foundReferences.Add("GUID:" + asmdefGuid);
                }
            }

            EditorUtility.ClearProgressBar();
            UpdateAsmdefFile(asmdefPath, foundReferences);
        }

        private static bool DoesAsmdefProvideNamespaces(string asmdefPath, HashSet<string> usingNamespaces)
        {
            var directoryPath = Path.GetDirectoryName(asmdefPath);
            var csFiles = GetCsFilesInDirectory(directoryPath);
            var declaredNamespaces = GetDeclaredNamespacesFromCsFiles(csFiles);

            declaredNamespaces.RemoveWhere(ns => _ignoredNamespacePrefixes.Any(ns.Contains));

            return usingNamespaces.Any(ns => declaredNamespaces.Contains(ns));
        }

        private static IEnumerable<string> GetCsFilesInDirectory(string directoryPath)
        {
            var guids = AssetDatabase.FindAssets("t:Script", new[] { directoryPath });
            return guids.Select(AssetDatabase.GUIDToAssetPath).Where(path => path.EndsWith(".cs"));
        }

        private static HashSet<string> GetUsingNamespacesFromCsFiles(IEnumerable<string> csFiles)
        {
            var usingRegex = new Regex(@"using\s+(?<namespace>[\w\.]+);");
            var namespaces = new HashSet<string>();

            foreach (var file in csFiles)
            {
                var content = File.ReadAllText(file);
                var matches = usingRegex.Matches(content);

                foreach (Match match in matches)
                {
                    namespaces.Add(match.Groups["namespace"].Value);
                }
            }

            return namespaces;
        }

        private static HashSet<string> GetDeclaredNamespacesFromCsFiles(IEnumerable<string> csFiles)
        {
            var namespaceRegex = new Regex(@"namespace\s+(?<namespace>[\w\.]+)");
            var namespaces = new HashSet<string>();

            foreach (var file in csFiles)
            {
                var content = File.ReadAllText(file);
                var matches = namespaceRegex.Matches(content);

                foreach (Match match in matches)
                {
                    namespaces.Add(match.Groups["namespace"].Value);
                }
            }

            return namespaces;
        }

        private static void UpdateAsmdefFile(string asmdefPath, HashSet<string> newReferences)
        {
            var asmdefContent = File.ReadAllText(asmdefPath);
            var asmdef = JsonUtility.FromJson<Asmdef>(asmdefContent);

            var currentReferences = new HashSet<string>(asmdef.references ?? Array.Empty<string>());

            foreach (var reference in newReferences)
            {
                currentReferences.Add(reference);
            }

            asmdef.references = currentReferences.ToArray();
            File.WriteAllText(asmdefPath, JsonUtility.ToJson(asmdef, true));

            AssetDatabase.Refresh();
        }
    }
}