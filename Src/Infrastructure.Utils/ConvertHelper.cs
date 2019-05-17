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
        /// 转换为整型数值
        /// </summary>
        /// <param name="value">数字字符串</param>
        /// <returns>成功，返回实际数值；失败，返回int.MinValue</returns>
        public static int ToInt(this string value)
        {
            if (!int.TryParse(value, out int result)) result = int.MinValue;

            return result;
        }
    }
}
