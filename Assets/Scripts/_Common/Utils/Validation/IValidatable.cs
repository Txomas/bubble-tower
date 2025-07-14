using Common.DataTypes;
using UnityEngine;

namespace Common.Utils
{
    public interface IValidatable
    {
        bool IsValid(Object context, IAddOnlyList<string> messages);
    }
}