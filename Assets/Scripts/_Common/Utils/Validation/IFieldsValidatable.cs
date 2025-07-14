using System.Collections.Generic;

namespace Common.Utils
{
    public interface IFieldsValidatable
    {
        IEnumerable<string> GetNullableFields()
        {
            yield break;
        }
    }
}