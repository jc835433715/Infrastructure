using Infrastructure.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BarcodeReader.Interface
{
    /// <summary>
    /// 条码读取器接口
    /// </summary>
    public interface IBarcodeReader : INotifyBarcodeReaderReceived, INotifyConnectionStateChanged
    {
        /// <summary>
        /// 条码读器名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 读取条码
        /// </summary>
        /// <returns>返回条码,若异常，返回BarcodeConst.ErrorBarcode</returns>
        string Read();
    }
}
