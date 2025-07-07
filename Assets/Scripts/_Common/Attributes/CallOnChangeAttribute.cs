using System;
using UnityEngine;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CallOnChangeAttribute : PropertyAttribute
    {
        public readonly string OnChangedCallbackName;
        
        public CallOnChangeAttribute(string onChangedCallbackName)
        {
            OnChangedCallbackName = onChangedCallbackName;
        }
    }
}