using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<FieldInfo> GetAllSerializableFields(this Type type)
        {
            return type.GetAllFields(IsFieldSerialized);
        }
        
        public static IEnumerable<FieldInfo> GetAllFields(this Type type, Func<FieldInfo, bool> selector)
        {
            var fieldInfos = new List<FieldInfo>();
            var currentType = type;

            do
            {
                var range = currentType
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(selector);
                fieldInfos.AddRange(range);

                currentType = currentType.BaseType;
            } while ((currentType != typeof(MonoBehaviour) || currentType != typeof(ScriptableObject)) && currentType != null);

            return fieldInfos.Distinct();
        }
        
        public static bool IsFieldSerialized(FieldInfo field)
        {
            return !Attribute.IsDefined(field, typeof(NonSerializedAttribute)) &&
                   (field.IsPublic || 
                    Attribute.IsDefined(field, typeof(SerializeField)) || 
                    Attribute.IsDefined(field, typeof(SerializeReference)));
        }
        
        public static bool ContainsAttribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), true).Length > 0;
        }
    }
}