
using Infrastructure.Serialization.Interface;
using Infrastructure.Serialization.Newtonsoft;
using Infrastructure.Serialization.YamlDotNet;

namespace Infrastructure.Serialization.Factory
{
    /// <summary>
    /// 转换器管理者
    /// </summary>
    public static class ConverterManager
    {
        /// <summary>
        /// 获取转换器
        /// </summary>
        /// <param name="converterType">转换器类型</param>
        /// <returns></returns>
        public static IConverter GetConverter(ConverterType converterType = ConverterType.NewtonsoftJsonConverter)
        {
            IConverter result = null;

            switch (converterType)
            {
                case ConverterType.NewtonsoftJsonConverter: result = new JsonConverter(); break;
                case ConverterType.NewtonsoftXmlConverter: result = new XmlConverter(); break;
                case ConverterType.YamlDotNetYamlConverter: result = new YamlConverter(); break;
                default: result = new JsonConverter(); break;
            }

            return result;
        }
    }

    /// <summary>
    /// 转换器类型
    /// </summary>
    public enum ConverterType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// NewtonsoftJson
        /// </summary>
        NewtonsoftJsonConverter,
        /// <summary>
        /// NewtonsoftXml
        /// </summary>
        NewtonsoftXmlConverter,
        /// <summary>
        ///   YamlDotNetYaml
        /// </summary>
        YamlDotNetYamlConverter,
    }
}
