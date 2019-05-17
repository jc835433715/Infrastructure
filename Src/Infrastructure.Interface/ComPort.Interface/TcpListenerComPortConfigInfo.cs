namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// TcpListener端口配置信息
    /// </summary>
    public class TcpListenerComPortConfigInfo
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public TcpListenerComPortConfigInfo()
        {
            this.Name = string.Empty;
            this.LocalIPAddress = string.Empty;
            this.LocalPort = -1;
            this.SendTimeout = 0;
            this.ReceiveTimeout = 0;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string LocalIPAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int LocalPort { get; set; }

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
