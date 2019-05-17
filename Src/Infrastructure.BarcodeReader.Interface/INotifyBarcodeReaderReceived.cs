using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BarcodeReader.Interface
{
    /// <summary>
    /// 通知条码接收事件
    /// </summary>
    public interface INotifyBarcodeReaderReceived
    {
        /// <summary>
        /// 条码接收事件
        /// </summary>
        event EventHandler<BarcodeReaderReceivedEventArgs> BarcoderReceived;
    }
}
