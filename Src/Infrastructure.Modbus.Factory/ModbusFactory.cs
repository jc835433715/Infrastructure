using Infrastructure.ComPort.Interface;
using Infrastructure.Modbus.Interface;
using Infrastructure.Modbus.NModbus4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modbus.Factory
{
    /// <summary>
    /// Modbus工厂
    /// </summary>
    public static class ModbusFactory
    {
        /// <summary>
        /// 创建Modbus主站
        /// </summary>
        /// <param name="tcpClientComPortConfigInfo">TcpClient端口配置信息</param>
        /// <returns>PlcModbus对象</returns>
        public static IModbus Create(TcpClientComPortConfigInfo tcpClientComPortConfigInfo)
        {
            return new ModbusMasterByNModbus4(tcpClientComPortConfigInfo);
        }

        /// <summary>
        /// 创建Modbus主站
        /// </summary>
        /// <param name="serialPortComPortConfigInfo">串口端口配置信息</param>
        /// <param name="modbusType">协议类型</param>
        /// <returns>PlcModbus对象</returns>
        public static IModbus Create(SerialPortComPortConfigInfo serialPortComPortConfigInfo, ModbusType modbusType)
        {
            ModbusTransportMode modbusTransportMode = ModbusTransportMode.Rtu;

            if (modbusType == ModbusType.ModbusMasterAscii) modbusTransportMode = ModbusTransportMode.Ascii;

            return new ModbusMasterByNModbus4(serialPortComPortConfigInfo, modbusTransportMode);
        }
    }
}
