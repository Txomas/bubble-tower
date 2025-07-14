using System.Collections.Generic;

namespace Common.DataTypes
{
    public interface IAddOnlyList<in T>
    {
        void Add(T item);
    }
    
    public class AddOnlyList<T> : List<T>, IAddOnlyList<T>
    {
    }
}