using System;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// Tcp客户端接收事件参数
    /// </summary>
    public class TcpClientAcceptedEventArgs : EventArgs
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public TcpClientAcceptedEventArgs()
        {
            this.TcpClientInfo = new TcpClientInfo();
        }

        /// <summary>
        /// Tcp客户端信息
        /// </summary>
        public TcpClientInfo TcpClientInfo { get; set; }
    }
}