using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.Plc.Keyence;
using Infrastructure.Plc.ManualSimulator;
using Infrastructure.Plc.Mitsubishi;
using Infrastructure.Plc.Omron;

namespace Infrastructure.Plc.Factory
{

    /// <summary>
    /// Plc工厂
    /// </summary>
    public static class PlcFactory
    {
        /// <summary>
        /// 创建Plc
        /// </summary>
        /// <param name="plcProtocolName">协议名称</param>
        /// <param name="tcpClientComPortConfigInfo">TcpClient端口配置信息</param>
        /// <param name="readTimeoutSeconds">读取超时</param>
        /// <param name="loggerFactory">日志工厂</param>
        /// <returns>Plc对象</returns>
        public static IPlc Create(PlcProtocolName plcProtocolName, TcpClientComPortConfigInfo tcpClientComPortConfigInfo, int readTimeoutSeconds = 3, ILoggerFactory loggerFactory = null)
        {
            IPlc result = null;
            IComPort comPort = new TcpClientComPort(tcpClientComPortConfigInfo, loggerFactory);

            if (plcProtocolName == PlcProtocolName.PlcOmronFinsTcp)
            {
                result = new PlcOmronFins(comPort, readTimeoutSeconds);
            }

            if (plcProtocolName == PlcProtocolName.PlcKeyenceUpperLink)
            {
                result = new PlcKeyenceUpperLink(comPort, readTimeoutSeconds);
            }

            if (plcProtocolName == PlcProtocolName.PlcMelsecMcBinary)
            {
                result = new PlcMelsecMcBinary(tcpClientComPortConfigInfo.RemoteIPAddress, (ushort)tcpClientComPortConfigInfo.RemotePort);
            }

            if (plcProtocolName == PlcProtocolName.PlcManualSimulator)
            {
                result = new PlcManualSimulator();
            }

            return result;
        }
    }
}
