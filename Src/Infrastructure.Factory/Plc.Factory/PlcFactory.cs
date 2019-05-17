using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.Plc.Keyence;
using Infrastructure.Plc.ManualSimulator;
using Infrastructure.Plc.Mitsubishi;
using Infrastructure.Plc.Modbus;
using Infrastructure.Plc.Omron;
using Infrastructure.Plc.Opc;

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
        /// <param name="loggerFactory">日志工厂</param>
        /// <returns>Plc对象</returns>
        public static IPlc Create(PlcProtocolName plcProtocolName, TcpClientComPortConfigInfo tcpClientComPortConfigInfo, ILoggerFactory loggerFactory = null)
        {
            IPlc result = null;
            IComPort comPort = new TcpClientComPort(tcpClientComPortConfigInfo, loggerFactory);

            if (plcProtocolName == PlcProtocolName.PlcOmronFinsTcp)
            {
                result = new PlcOmronFins(comPort);
            }

            if (plcProtocolName == PlcProtocolName.PlcKeyenceUpperLink)
            {
                result = new PlcKeyenceUpperLink(comPort);
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

        /// <summary>
        /// 创建PlcOpc
        /// </summary>
        /// <param name="uriString">uri</param>
        /// <param name="plcOpcClientOptions">Opc客户端配置</param>
        /// <returns>PlcOpc对象</returns>
        public static IPlcOpc Create(string uriString, PlcOpcClientOptions plcOpcClientOptions = null)
        {
            return new PlcOpcByOpcUaHelper(uriString, plcOpcClientOptions); ;
        }

        /// <summary>
        /// 创建PlcModbus
        /// </summary>
        /// <param name="tcpClientComPortConfigInfo">TcpClient端口配置信息</param>
        /// <returns>PlcModbus对象</returns>
        public static IPlcModbus Create(TcpClientComPortConfigInfo tcpClientComPortConfigInfo)
        {
            return new PlcModbusMaster(tcpClientComPortConfigInfo);
        }

        /// <summary>
        /// 创建PlcModbus
        /// </summary>
        /// <param name="serialPortComPortConfigInfo">串口端口配置信息</param>
        /// <param name="plcProtocolName">协议名称</param>
        /// <returns>PlcModbus对象</returns>
        public static IPlcModbus Create(SerialPortComPortConfigInfo serialPortComPortConfigInfo, PlcProtocolName plcProtocolName)
        {
            PlcModbusTransportMode modbusTransportMode = PlcModbusTransportMode.Rtu;

            if (plcProtocolName == PlcProtocolName.PlcModbusMasterAscii) modbusTransportMode = PlcModbusTransportMode.Ascii;

            return new PlcModbusMaster(serialPortComPortConfigInfo, modbusTransportMode);
        }

    }
}
