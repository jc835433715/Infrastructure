using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// Plc通讯端口类型
    /// </summary>
    public enum PlcComPortType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// Tcp客户端
        /// </summary>
        TcpClientComPort,
        /// <summary>
        /// Tcp服务端
        /// </summary>
        TcpListenerComPort,
        /// <summary>
        /// 串口
        /// </summary>
        SerialPortComPort
    }
}
