using System;

namespace Infrastructure.Serialization.Interface
{
    /// <summary>
    /// 序列化转换器
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value">序列化对象</param>
        /// <returns>序列化字符串</returns>
        string Serialize(object value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="value">序列化字符串</param>
        /// <returns>反序列化对象</returns>
        T Deserialize<T>(string value);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value">序列化字符串</param>
        /// <param name="type">反序列化类型</param>
        /// <returns>反序列化对象</returns>
        object Deserialize(string value, Type type);
    }
}
