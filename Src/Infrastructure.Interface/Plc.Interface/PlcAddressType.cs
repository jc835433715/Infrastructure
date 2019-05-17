using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// 地址类型
    /// </summary>
    public enum PlcAddressType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 布尔类型
        /// </summary>
        Boolean,
        /// <summary>
        /// 短整型
        /// </summary>
        Short,
        /// <summary>
        /// 无符号短整型
        /// </summary>
        Ushort,
        /// <summary>
        /// 整型
        /// </summary>
        Int,
        /// <summary>
        /// 浮点类型
        /// </summary>
        Float,
        /// <summary>
        /// 字符串类型
        /// </summary>
        String,
    }
}
