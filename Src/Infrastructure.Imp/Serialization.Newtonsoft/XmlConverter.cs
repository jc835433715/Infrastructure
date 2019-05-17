using Infrastructure.Serialization.Interface;
using Newtonsoft.Json;
using System;
using System.Xml;

namespace Infrastructure.Serialization.Newtonsoft
{
    public class XmlConverter : IConverter
    {
        public string Serialize(object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var doc = JsonConvert.DeserializeXmlNode(json) as XmlDocument;

            return doc.OuterXml;
        }

        public T Deserialize<T>(string value)
        {
            T result = default(T);
            XmlDocument doc = new XmlDocument();
            string json = string.Empty;

            doc.LoadXml(value);
            json = JsonConvert.SerializeXmlNode(doc);
            result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }

        public object Deserialize(string value, Type type)
        {
            object result = null;
            XmlDocument doc = new XmlDocument();
            string json = string.Empty;

            doc.LoadXml(value);
            json = JsonConvert.SerializeXmlNode(doc);
            result = JsonConvert.DeserializeObject(json, type);

            return result;
        }
    }
}
