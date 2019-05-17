namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// TcpClient端口配置信息
    /// </summary>
    public class TcpClientComPortConfigInfo
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public TcpClientComPortConfigInfo()
        {
            this.Name = string.Empty;
            this.LocalIPAddress = string.Empty;
            this.LocalPort = -1;
            this.RemoteIPAddress = string.Empty;
            this.RemotePort = -1;
            this.SendTimeout = 0;
            this.ReceiveTimeout = 0;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 本地IP地址
        /// </summary>
        public string LocalIPAddress { get; set; }

        /// <summary>
        /// 本地端口号
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// 服务器端IP地址
        /// </summary>
        public string RemoteIPAddress { get; set; }

        /// <summary>
        /// 服务器端端口号
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 发送时延
        /// </summary>
        public int SendTimeout { get; set; }

        /// <summary>
        /// 接收时延
        /// </summary>
        public int ReceiveTimeout { get; set; }

    }
}
