using System;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 数据已接收事件参数
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public DataReceivedEventArgs()
        {
            this.BytesToRead = 0;
        }

        /// <summary>
        ///可读取的字节数
        /// </summary>
        public int BytesToRead { get; set; }
    }
}
