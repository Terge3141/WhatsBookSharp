using System;
using System.IO;
using System.Xml.Serialization;

namespace Helper
{
    public class Serializer
    {
        public static T DeserializeFromXml<T>(string xmlstr)
        {
            var sr = new StringReader(xmlstr);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(sr);
        }

        public static string SerializeToXml<T>(T obj)
        {
            var sw = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(sw, obj);
            return sw.ToString();
        }
    }
}

