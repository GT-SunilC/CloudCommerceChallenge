using Microsoft.VisualBasic.FileIO;
using SourceConverter.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace SourceConverter
{
    public sealed class DelimitedFileConverter: ISourceConverter
    {
        #region Constructor and Private Member Variables
        private string[] _delimiters;

        public DelimitedFileConverter(string[] delimiters)
        {
            _delimiters = delimiters;
        }
        #endregion

        #region ISourceConverter Interface Methods
        public List<ExpandoObject> DeSerializeObject(string path)
        {
            var delimitedFileObjects = new List<ExpandoObject>();
            try
            {
                if (File.Exists(path))
                {
                    using (var textFieldParser = new TextFieldParser(path))
                    {
                        textFieldParser.TextFieldType = FieldType.Delimited;
                        textFieldParser.Delimiters = _delimiters;
                        textFieldParser.HasFieldsEnclosedInQuotes = true;

                        var header = true;
                        var headers = new List<string>();

                        while (!textFieldParser.EndOfData)
                        {
                            if (header)
                            {
                                headers = textFieldParser.ReadFields().ToList();
                                header = false;
                            }
                            else
                            {
                                delimitedFileObjects.Add(ParseLine(textFieldParser.ReadFields(), headers));
                            }

                        }

                    }
                }
                else
                    throw new FileNotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return delimitedFileObjects;
        }

        string ISourceConverter.SerializeObject(List<ExpandoObject> source)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Private Methods

        private dynamic ParseLine(string[] fields, List<string> headers)
        {
            dynamic delimitedLineObject = new ExpandoObject();
            var expandoDictionary = delimitedLineObject as IDictionary<string, object>;

            for (var index = 0; index < fields.Length; index++)
            {
                var header = headers[index];
                var value = fields[index];
                if (header.Contains("_"))
                {
                    var queue = new Queue<string>(header.Split("_"));
                    CreateNestedObject(queue, value, expandoDictionary);
                }
                else
                    expandoDictionary.Add(header, value);
            }

            return delimitedLineObject;
        }

        private dynamic CreateNestedObject(Queue<string> nestedHeaders, object value, IDictionary<string, object> expandoDictionary)
        {
            var header = nestedHeaders.Dequeue();
            if (nestedHeaders.Count == 0)
            {
                expandoDictionary.Add(header, value);
            }
            else
            {
                var parentExpando = new ExpandoObject();
                var parentDictionary = parentExpando as IDictionary<string, object>;
                if (expandoDictionary.ContainsKey(header))
                {
                    var existingObject = expandoDictionary[header] as IDictionary<string, object>;
                    var additionalObject = CreateNestedObject(nestedHeaders, value, parentDictionary) as IDictionary<string, object>;
                    var merged = new List<IDictionary<string, object>>();
                    merged.Add(existingObject);
                    merged.Add(additionalObject);

                    expandoDictionary.Remove(header);
                    expandoDictionary.Add(header, merged.SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value));
                }
                else
                    expandoDictionary.Add(header, CreateNestedObject(nestedHeaders, value, parentDictionary));
            }

            return expandoDictionary;
            
        }

        //private dynamic CreateNestedObject(Queue<string> nestedHeaders, object value, IDictionary<string, object> expandoDictionary)
        //{
        //    var header = nestedHeaders.Dequeue();
        //    if (nestedHeaders.Count > 0)
        //    {
        //        var parentExpando = new ExpandoObject();
        //        var parentDictionary = parentExpando as IDictionary<string, object>;
        //        if (expandoDictionary.ContainsKey(header))
        //        {
        //            var existingObject = expandoDictionary[header] as IDictionary<string, object>;
        //            var additionalObject = CreateNestedObject(nestedHeaders, value, parentDictionary) as IDictionary<string, object>;
        //            var merged = new List<IDictionary<string, object>>();
        //            merged.Add(existingObject);
        //            merged.Add(additionalObject);

        //            expandoDictionary.Remove(header);
        //            expandoDictionary.Add(header, merged.SelectMany(x => x).ToDictionary(x => x.Key, y => y.Value));
        //        }
        //        else
        //            expandoDictionary.Add(header, CreateNestedObject(nestedHeaders, value, parentDictionary));
        //    }

        //    dynamic leafObject = new ExpandoObject();
        //    var leafDictionary = leafObject as IDictionary<string, object>;

        //    leafDictionary.Add(header, value);

        //    return leafObject;

        //}
        #endregion
    }
}
