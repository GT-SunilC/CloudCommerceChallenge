using System.Collections.Generic;
using System.Dynamic;

namespace SourceConverter.Interfaces
{
    public interface ISourceConverter
    {
        public List<ExpandoObject> DeSerializeObject(string source);

        public string SerializeObject(List<ExpandoObject> source);
    }
}
