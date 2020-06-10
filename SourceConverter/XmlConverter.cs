using SourceConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;

namespace SourceConverter
{
    public sealed class XmlConverter : ISourceConverter
    {
        #region ISourceConverter Interface Methods
        public string SerializeObject(List<ExpandoObject> values)
        {
            var root = new XElement("root");
            var document = new XDocument(new XDeclaration("1.0","utf-8","yes"),root);

            foreach(var value in values)
            {
                var line = new XElement("line");
                root.Add(ParseObject(value, line));
            }
            
            return document.ToString();
        }

        public List<ExpandoObject> DeSerializeObject(string source)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Private Methods
        private static XElement ParseObject(IDictionary<string,object> value, XElement parentElement)
        {
            try
            {
                foreach (KeyValuePair<string, object> keyValuePair in value)
                {
                    if (keyValuePair.Value is IDictionary<string, object>)
                    {
                        parentElement.Add(ParseObject(keyValuePair.Value as IDictionary<string, object>, new XElement(keyValuePair.Key)));
                    }
                    else
                        parentElement.Add(new XElement(keyValuePair.Key, keyValuePair.Value));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return parentElement;
        }

        #endregion
    }
}
