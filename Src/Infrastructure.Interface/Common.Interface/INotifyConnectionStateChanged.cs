using System;

namespace Infrastructure.Common.Interface
{
    /// <summary>
    /// 通知连接状态改变接口
    /// </summary>
    public interface INotifyConnectionStateChanged
    {
        /// <summary>
        /// 连接状态改变事件
        /// </summary>
        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

    }
}
