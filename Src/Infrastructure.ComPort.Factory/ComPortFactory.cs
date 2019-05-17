using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Imp.SerialPort;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;

namespace Infrastructure.ComPort.Factory
{
    /// <summary>
    /// 通讯端口工厂
    /// </summary>
    public static class ComPortFactory
    {
        /// <summary>
        ///创建通讯端口
        /// </summary>
        /// <param name="serialPortConfigInfo">串口端口配置信息</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <returns>通讯端口</returns>
        public static IComPort Create(SerialPortComPortConfigInfo serialPortConfigInfo, ILoggerFactory loggerFactory = null)
        {
            return new SerialPortComPort(serialPortConfigInfo, loggerFactory);
        }

        /// <summary>
        /// 创建通讯端口
        /// </summary>
        /// <param name="tcpClientComPortConfigInfo">TcpClient端口配置信息</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <returns>通讯端口</returns>
        public static IComPort Create(TcpClientComPortConfigInfo tcpClientComPortConfigInfo, ILoggerFactory loggerFactory = null)
        {
            return new TcpClientComPort(tcpClientComPortConfigInfo, loggerFactory);
        }

        /// <summary>
        /// 创建通讯端口
        /// </summary>
        /// <param name="tcpListenerComPortConfigInfo">TcpListener端口配置信息</param>
        /// <param name="tcpListenerComPortType">TcpListenerComPort类型</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <returns>通讯端口</returns>
        public static IComPort Create(TcpListenerComPortConfigInfo tcpListenerComPortConfigInfo, TcpListenerComPortType tcpListenerComPortType, ILoggerFactory loggerFactory = null)
        {
            return new TcpListenerComPort(tcpListenerComPortConfigInfo, tcpListenerComPortType, loggerFactory);
        }
    }
}
