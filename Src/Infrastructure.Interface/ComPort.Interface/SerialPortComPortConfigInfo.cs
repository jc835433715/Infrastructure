using System.IO.Ports;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 串口端口配置信息
    /// </summary>
    public class SerialPortComPortConfigInfo
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public SerialPortComPortConfigInfo()
        {
            this.Name = string.Empty;
            this.PortName = string.Empty;
            this.BaudRate = 0;
            this.Parity = Parity.None;
            this.DataBits = 0;
            this.StopBits = StopBits.None;
            this.SendTimeout = 0;
            this.ReceiveTimeout = 0;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 端口名称
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; set; }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; }

        /// <summary>
        /// 发送时延
        /// </summary>
        public int SendTimeout { get; set; }

        /// <summary>
        /// 接收时延
        /// </summary>
        public int ReceiveTimeout { get; set; }

        /// <summary>
        /// 获取所串口名
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
    }
}
