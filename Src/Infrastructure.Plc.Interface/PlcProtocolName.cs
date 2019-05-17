using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// 协议名称
    /// </summary>
    public enum PlcProtocolName
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 欧姆龙FinsTcp
        /// </summary>
        PlcOmronFinsTcp,
        /// <summary>
        /// 基恩士上位链路
        /// </summary>
        PlcKeyenceUpperLink,
        /// <summary>
        /// 三菱Mc（Binary）
        /// </summary>
        PlcMelsecMcBinary,
        /// <summary>
        /// Plc手动模拟
        /// </summary>
        PlcManualSimulator,
        /// <summary>
        /// Opc
        /// </summary>
        PlcOpc,
        /// <summary>
        /// Modbus主站Tcp
        /// </summary>
        PlcModbusMasterTcp,
        /// <summary>
        /// Modbus主站Rtu
        /// </summary>
        PlcModbusMasterRtu,
        /// <summary>
        /// Modbus主站Ascii
        /// </summary>
        PlcModbusMasterAscii
    }
}
