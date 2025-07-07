using System.Reflection;
using Common.Extensions;
using UnityEditor;
using UnityEngine;

namespace Common.Attributes
{
    [InitializeOnLoad]
    public static class InspectorButtonAction
    {
        private const int ButtonHeight = 40;
        private const int Space = 5;
        
        static InspectorButtonAction()
        {
            MonoBehaviourEditor.RegisterActionForAttribute<InspectorButtonAttribute>(Action);
        }
        
        private static void Action(MemberInfo memberInfo, SerializedObject serializedObject)
        {
            GUILayout.Space(Space);
                
            var attribute = memberInfo.GetCustomAttribute<InspectorButtonAttribute>();

            if (attribute != null)
            {
                var name = string.IsNullOrWhiteSpace(attribute.Name) ? memberInfo.Name.SplitCamelCase() : attribute.Name;
                
                if (GUILayout.Button(name, GUILayout.Height(ButtonHeight)))
                {
                    var methodInfo = (MethodInfo) memberInfo;
                    methodInfo.Invoke(serializedObject.targetObject, null);
                }
            }
            
        }
    }
}