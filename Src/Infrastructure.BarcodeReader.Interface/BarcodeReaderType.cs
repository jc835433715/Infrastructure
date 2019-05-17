
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BarcodeReader.Interface
{
    /// <summary>
    /// 条码读取器类型
    /// </summary>
    public enum BarcodeReaderType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// Plc
        /// </summary>
        Plc,
        /// <summary>
        /// TcpClient
        /// </summary>
        TcpClient,
        /// <summary>
        /// 串口
        /// </summary>
        SerialPort,
        /// <summary>
        /// UI
        /// </summary>
        UI
    }
}
