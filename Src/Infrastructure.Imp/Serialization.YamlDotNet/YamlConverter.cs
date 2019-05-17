using Infrastructure.Serialization.Interface;
using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Infrastructure.Serialization.YamlDotNet
{
    public class YamlConverter : IConverter
    {
        public T Deserialize<T>(string value)
        {
            var input = new StringReader(value);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<T>(input);
        }

        public object Deserialize(string value, Type type)
        {
            var input = new StringReader(value);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize(value, type);
        }

        public string Serialize(object value)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return serializer.Serialize(value);
        }
    }
}
