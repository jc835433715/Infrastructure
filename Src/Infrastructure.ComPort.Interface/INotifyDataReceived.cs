using System;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 通知数据已接收接口
    /// </summary>
    public interface INotifyDataReceived
    {
        /// <summary>
        /// 数据已接收事件
        /// </summary>
        event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}
