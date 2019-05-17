using System;
using System.Net.Sockets;

namespace Infrastructure.ComPort.Imp.Net
{
    /// <summary>
    /// KeepAlive帮助者
    /// 参考资料：
    /// http://www.cnblogs.com/yjf512/p/5354055.html
    /// https://www.jianshu.com/p/a4beee06220c
    /// </summary>
    static class KeepAliveHelper
    {
        /// <summary>
        /// 设置KeepAlive
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="keepAliveTime">开始首次KeepAlive探测前的TCP空闭时间，单位：ms</param>
        /// <param name="keepAliveInterval">两次KeepAlive探测间的时间间隔，单位：ms</param>
        public static void SetKeepAlive(Socket socket, int keepAliveTime, int keepAliveInterval)
        {
            byte[] buffer = new byte[12];

            BitConverter.GetBytes(1).CopyTo(buffer, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(buffer, 4);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(buffer, 8);

            socket.IOControl(IOControlCode.KeepAliveValues, buffer, null);
        }
    }
}
