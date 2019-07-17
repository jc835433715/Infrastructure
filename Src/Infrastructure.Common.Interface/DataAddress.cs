using System;

namespace Infrastructure.Common.Interface
{
    /// <summary>
    /// 数据地址
    /// </summary>
    [Serializable]
    public struct DataAddress
    {
        static DataAddress()
        {
            Empty = new DataAddress();
        }

        /// <summary>
        /// 地址分隔符
        /// </summary>
        public const char ValueSeparator = '、';

        /// <summary>
        /// 空地址
        /// </summary>
        public static readonly DataAddress Empty;

        /// <summary>
        /// 地址名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址值：例如：
        /// FinsTcp：
        ///     E0_0001
        /// UpperLink：
        ///     EM0001
        /// Opc:
        ///     X.Y
        ///     X.Y、M.N
        ///     ns=1;s=t|DEVICE_B4_QHJ_ParameterCheck_SetValue_BatteryType
        ///     ns=1;s=t|DEVICE_B4_QHJ_ParameterCheck_SetValue_BatteryType、ns=1;s=t|DEVICE_B4_HHJ_ParameterCheck_SetValue_VacuumTime
        /// Modbus：
        ///     1:DI:100
        ///     1:DO:100
        ///     1:AI:100
        ///     1:AO:100
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 地址类型
        /// </summary>
        public DataAddressType Type { get; set; }

        /// <summary>
        /// 地址偏移，从0开始
        /// Type为Int、Short、Ushort、Int、Float，偏移等同数据个数
        /// Type为String，偏移为字符数
        /// Type为Boolean，[0,15]:偏移为位数,-1:整个字表示一个Boolean;Modbus的话，为IO数
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return (Name ?? string.Empty).GetHashCode() | (Value ?? string.Empty).GetHashCode() | Type.GetHashCode() | Offset.GetHashCode();
        }

        /// <summary>
        /// 当前对象的字符串
        /// </summary>
        /// <returns>返回字符串</returns>
        public override string ToString()
        {
            char paddingChar = ' ';

            return
                $"地址名：{(Name ?? string.Empty).PadRight(30, paddingChar) }," +
                $"地址类型：{Type.ToString().PadRight(10, paddingChar)  }," +
                $"地址值：{(Value ?? string.Empty).PadRight(10, paddingChar) }," +
                $"地址偏移：{Offset.ToString().PadRight(5, paddingChar)}";
        }
    }
}
