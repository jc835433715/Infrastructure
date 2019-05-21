using Infrastructure.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// Plc扩展
    /// </summary>
    public static class PlcExtensions
    {
        /// <summary>
        /// 读取单个值
        /// </summary>
        /// <param name="plc">Plc接口</param>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>返回值</returns>
        public static TValue ReadSingle<TValue>(this IPlc plc, DataAddress address)
        {
            return plc.Read<TValue>(address).Single();
        }

        /// <summary>
        /// 写入单个值
        /// </summary>
        /// <param name="plc">Plc接口</param>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public static void WriteSingle<TValue>(this IPlc plc, DataAddress address, TValue value)
        {
            plc.Write(address, new TValue[] { value });
        }
    }
}
