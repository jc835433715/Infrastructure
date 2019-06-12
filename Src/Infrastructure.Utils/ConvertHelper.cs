using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Utils
{
    /// <summary>
    /// 转换帮助者
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// 转换为十六进制字符串
        /// </summary>
        /// <param name="value">byte数组</param>
        /// <returns>返回十六进制字符串</returns>
        public static string ToString(byte[] value)
        {
            return string.Join(" ", value.Select(e => e.ToString("X2"))
                .ToArray());
        }

        /// <summary>
        /// 转换为byte数组
        /// </summary>
        /// <param name="value">十六进制字符串</param>
        /// <returns>返回byte数组</returns>
        public static byte[] ToBytes(string value)
        {
            return value.Split(new string[] { " ", ",", "，" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => System.Convert.ToByte(e, 16))
                .ToArray();
        }

        /// <summary>
        /// 转换为短整
        /// </summary>
        /// <param name="value">数字字符串</param>
        /// <returns>实际数值</returns>
        public static short ToShort(this string value)
        {
            return short.Parse(value);
        }

        /// <summary>
        /// 转换为无符号短整
        /// </summary>
        /// <param name="value">数字字符串</param>
        /// <returns>实际数值</returns>
        public static ushort ToUshort(this string value)
        {
            return ushort.Parse(value);
        }

        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="value">数字字符串</param>
        /// <returns>实际数值</returns>
        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }

        /// <summary>
        /// 转换为浮点型
        /// </summary>
        /// <param name="value">数字字符串</param>
        /// <returns>实际数值</returns>
        public static float ToSingle(this string value)
        {
            return float.Parse(value);
        }

    }
}
