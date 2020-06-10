using Newtonsoft.Json;
using SourceConverter.Interfaces;
using System.Collections.Generic;
using System.Dynamic;

namespace SourceConverter
{
    public sealed class JsonConverter : ISourceConverter
    {
        #region ISourceConverter Interface Methods
        public string SerializeObject(List<ExpandoObject> values)
        {
            return JsonConvert.SerializeObject(values, Newtonsoft.Json.Formatting.Indented);
        }

        public List<ExpandoObject> DeSerializeObject(string source)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
