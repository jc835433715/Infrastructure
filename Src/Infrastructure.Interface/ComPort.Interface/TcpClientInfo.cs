namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// Tcp客户端信息
    /// </summary>
    public class TcpClientInfo
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public TcpClientInfo()
        {
            this.IPAddress = string.Empty;
            this.ComPort = null;
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 通讯端口
        /// </summary>
        public IComPort ComPort { get; set; }
    }
}
