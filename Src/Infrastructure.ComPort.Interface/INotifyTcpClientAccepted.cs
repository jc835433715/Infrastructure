using System;
using System.Collections.Generic;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 通知Tcp客户端接收接口
    /// </summary>
    public interface INotifyTcpClientAccepted
    {
        /// <summary>
        /// Tcp客户端接收事件
        /// </summary>
        event EventHandler<TcpClientAcceptedEventArgs> TcpClientAccepted;

        /// <summary>
        /// 所有Tcp客户端
        /// </summary>
        IEnumerable<TcpClientInfo> TcpClientInfos { get; }
    }
}
