using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Extensions;
using UnityEditor;
using UnityEngine;

namespace Common.Attributes
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourEditor : Editor
    {
        private static readonly Dictionary<Type, IEnumerable<MethodInfo>> _methodsByType = new();
        private static readonly Dictionary<Type, IEnumerable<FieldInfo>> _fieldsByType = new();
        
        private static readonly Dictionary<Type, Action<MemberInfo, SerializedObject>> _actionsByAttributeType = new();
        
        private void OnEnable()
        {
            var targetType = target.GetType();

            if (!_methodsByType.ContainsKey(targetType))
            {
                var methods = targetType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(info => info.GetCustomAttributes<InspectorCustomAttribute>(true).Any());

                _methodsByType[targetType] = methods;
            }
            
            if (!_fieldsByType.ContainsKey(targetType))
            {
                var fieldInfos = targetType
                    .GetAllSerializableFields()
                    .Where(info => info.GetCustomAttributes<InspectorCustomAttribute>(true).Any())
                    .ToList();

                if (fieldInfos.Count > 0)
                {
                    _fieldsByType.Add(targetType, fieldInfos);
                }
            }
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var targetType = target.GetType();
            var memberInfos = new List<MemberInfo>();

            if (_fieldsByType.TryGetValue(targetType, out var fields))
            {
                memberInfos.AddRange(fields);
            }
            
            if (_methodsByType.TryGetValue(targetType, out var methods))
            {
                memberInfos.AddRange(methods);
            }
            
            foreach (var memberInfo in memberInfos)
            {
                var actions = memberInfo.CustomAttributes
                    .Select(attribute => attribute.AttributeType)
                    .Where(type => _actionsByAttributeType.ContainsKey(type))
                    .Select(type => _actionsByAttributeType[type]);
                    
                foreach (var action in actions)
                {
                    action(memberInfo, serializedObject);
                }
            }
        }
        
        public static void RegisterActionForAttribute<TAttribute>(Action<MemberInfo, SerializedObject> action)
            where TAttribute : InspectorCustomAttribute
        {
            _actionsByAttributeType[typeof(TAttribute)] = action;
        }
    }
}