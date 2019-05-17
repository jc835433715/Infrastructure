using System;

namespace Infrastructure.Common.Interface
{
    /// <summary>
    /// 连接状态改变事件参数
    /// </summary>
    public class ConnectionStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public ConnectionStateChangedEventArgs()
        {
            this.IsConnected = false;
            this.Name = string.Empty;
        }

        /// <summary>
        /// 连接名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已连接，默认false
        /// </summary>
        public bool IsConnected
        {
            get;
            set;
        }
    }
}
