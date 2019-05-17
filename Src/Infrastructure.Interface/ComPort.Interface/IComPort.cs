using Infrastructure.Common.Interface;
using System;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 通讯端口接口
    /// </summary>
    public interface IComPort : IDisposable, INotifyConnectionStateChanged
    {

        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected
        {
            get;
        }

        /// <summary>
        /// 端口名
        /// </summary>
        string Name { get; }

        /// <summary>
        ///可读取的字节数
        /// </summary>
        int BytesToRead
        {
            get;
        }

        /// <summary>
        /// 打开自动重连
        /// </summary>
        ///<param name="isAsync">是否异步</param>
        void Open(bool isAsync = true);

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">字节数</param>
        void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <param name="offset">字节偏移量</param>
        /// <param name="count">最多读取的字节数</param>
        /// <returns>返回实际读取字节数</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}
