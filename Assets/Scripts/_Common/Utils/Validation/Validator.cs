using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.DataTypes;
using Common.Extensions;
using UnityEngine;

namespace Common.Utils
{
    public static class Validator
    {
        public static bool ValidateObjects(IEnumerable<Object> objectsToValidate)
        {
            var result = true;
            
            foreach (var objectToValidate in objectsToValidate)
            {
                if (objectToValidate is IValidatable validatableObject && !ValidateObject(validatableObject, objectToValidate))
                {
                    result = false;
                }

                if (objectToValidate is IFieldsValidatable fieldsValidatable && !IsAllFieldsValid(fieldsValidatable, objectToValidate))
                {
                    result = false;
                }
            }
            
            return result;
        }

        public static bool IsAllFieldsValid(IFieldsValidatable target, Object context)
        {
            var isSuccessful = true;
            
            var excludedFields = target.GetNullableFields().ToList();
                
            var fields = target
                .GetType()
                .GetAllSerializableFields();
                
            foreach (var info in fields)
            {
                var value = info.GetValue(target);

                if (value is Object unityObject)
                {
                    context = unityObject;
                }

                if (value == null && !excludedFields.Contains(info.Name))
                {
                    Debug.LogError($"Field {info.Name} in {context.name} is null", context);
                    isSuccessful = false;
                    continue;
                }
                
                if (value is IValidatable validatable && !ValidateObject(validatable, context, info))
                {
                    isSuccessful = false;
                }
                
                if (value is IFieldsValidatable fieldsValidatable && !IsAllFieldsValid(fieldsValidatable, context))
                {
                    isSuccessful = false;
                }
                
                if (value is IEnumerable validatables)
                {
                    foreach (var item in validatables)
                    {
                        if (item is Object newContext)
                        {
                            context = newContext;
                        }
                        
                        if (item is IValidatable validatableItem && !ValidateObject(validatableItem, context, info))
                        {
                            isSuccessful = false;
                        }
                        
                        if (item is IFieldsValidatable fieldsValidatableItem && !IsAllFieldsValid(fieldsValidatableItem, context))
                        {
                            isSuccessful = false;
                        }
                    }
                }
            }
            
            return isSuccessful;
        }

        private static bool ValidateObject(IValidatable validatable, Object context, FieldInfo fieldInfo = null)
        {
            var messages = new AddOnlyList<string>();
            
            if (!validatable.IsValid(context, messages))
            {
                foreach (var message in messages)
                {
                    if (fieldInfo == null)
                    {
                        Debug.LogError($"{context.name}: {message}", context);
                    }
                    else
                    {
                        Debug.LogError($"{context.name}.{fieldInfo.Name}: {message}", context);
                    }
                }
                
                return false;
            }
            
            return true;
        }
    }
}