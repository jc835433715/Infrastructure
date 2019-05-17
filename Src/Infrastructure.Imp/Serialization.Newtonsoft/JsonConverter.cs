using Infrastructure.Serialization.Interface;
using Newtonsoft.Json;
using System;

namespace Infrastructure.Serialization.Newtonsoft
{
    public class JsonConverter : IConverter
    {
        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }
    }
}
