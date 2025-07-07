using System.Collections.Generic;
using UnityEngine;

namespace Common.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> list, int? seed = null)
        {
            var random = seed != null ? new System.Random(seed.Value) : new System.Random();
            return list.Shuffle(random);
        }
        
        public static List<T> Shuffle<T>(this List<T> list, System.Random random)
        {  
            var count = list.Count; 
            
            while (count-- > 1) 
            {  
                var index = random.Next(count + 1);  
                (list[index], list[count]) = (list[count], list[index]);
            }
            
            return list;
        }
    
        public static T GetClamped<T>(this List<T> list, int index)
        {
            index = Mathf.Clamp(index, 0, list.Count - 1);
            
            return list[index];
        }
    
        public static T GetRandom<T>(this List<T> list, System.Random random)
        {
            var index = random.Next(0, list.Count);
            return list[index];
        }
    }
}