using System;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButtonAttribute : InspectorCustomAttribute
    {
        public string Name { get; }

        public InspectorButtonAttribute(string name = null)
        {
            Name = name;
        }
    }
}