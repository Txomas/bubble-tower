using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Common.Attributes
{
    [CustomPropertyDrawer(typeof(CallOnChangeAttribute))]
    public class CallOnChangeAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            
            if (!EditorGUI.EndChangeCheck()) return;

            var targetObject = property.serializedObject.targetObject;
        
            var callAttribute = (CallOnChangeAttribute)attribute;
            var methodName = callAttribute.OnChangedCallbackName;

            var classType = targetObject.GetType();
            var methodInfo = classType.GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            property.serializedObject.ApplyModifiedProperties();
        
            if (!methodInfo.GetParameters().Any())
            {
                methodInfo.Invoke(targetObject, null);
            }
        }
    }
}