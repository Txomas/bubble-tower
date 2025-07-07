using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    public class EnumExtensions
    {
        public static List<T> GetEnumList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        
        public static IEnumerable<string> GetEnumNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }
    }
}