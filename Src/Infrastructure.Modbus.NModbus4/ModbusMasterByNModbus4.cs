using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Modbus.Interface;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Modbus.NModbus4
{
    public class ModbusMasterByNModbus4 : IModbus
    {
        public ModbusMasterByNModbus4()
        {
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(ModbusMasterByNModbus4));
        }

        public ModbusMasterByNModbus4(TcpClientComPortConfigInfo tcpClientComPortConfigInfo) :
            this()
        {
            this.tcpClientComPortConfigInfo = tcpClientComPortConfigInfo;
        }

        public ModbusMasterByNModbus4(SerialPortComPortConfigInfo serialPortComPortConfigInfo, ModbusTransportMode plcModbusTransportMode = ModbusTransportMode.Rtu)
        {
            this.serialPortComPortConfigInfo = serialPortComPortConfigInfo;
            this.plcModbusTransportMode = plcModbusTransportMode;
        }


        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Initialize()
        {
            try
            {
                if (tcpClientComPortConfigInfo != null)
                {
                    TcpClient client = null;

                    if (string.IsNullOrEmpty(tcpClientComPortConfigInfo.LocalIPAddress))
                    {
                        client = new TcpClient()
                        {
                            ReceiveTimeout = tcpClientComPortConfigInfo.ReceiveTimeout,
                            SendTimeout = tcpClientComPortConfigInfo.SendTimeout
                        };
                    }
                    else
                    {
                        client = new TcpClient(new IPEndPoint(IPAddress.Parse(tcpClientComPortConfigInfo.LocalIPAddress), tcpClientComPortConfigInfo.LocalPort))
                        {
                            ReceiveTimeout = tcpClientComPortConfigInfo.ReceiveTimeout,
                            SendTimeout = tcpClientComPortConfigInfo.SendTimeout
                        };
                    }

                    client.Connect(tcpClientComPortConfigInfo.RemoteIPAddress, tcpClientComPortConfigInfo.RemotePort);

                    master = ModbusIpMaster.CreateIp(client);

                    connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, client);
                }

                if (serialPortComPortConfigInfo != null)
                {
                    SerialPort port = new SerialPort()
                    {
                        PortName = serialPortComPortConfigInfo.PortName,
                        BaudRate = serialPortComPortConfigInfo.BaudRate,
                        DataBits = serialPortComPortConfigInfo.DataBits,
                        Parity = serialPortComPortConfigInfo.Parity,
                        StopBits = serialPortComPortConfigInfo.StopBits,
                        WriteTimeout = serialPortComPortConfigInfo.SendTimeout,
                        ReadTimeout = serialPortComPortConfigInfo.ReceiveTimeout
                    };

                    port.Open();

                    if (plcModbusTransportMode == ModbusTransportMode.Rtu)
                    {
                        master = ModbusSerialMaster.CreateRtu(port);
                    }

                    if (plcModbusTransportMode == ModbusTransportMode.Ascii)
                    {
                        master = ModbusSerialMaster.CreateAscii(port);
                    }

                    connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, port);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("建立与PLC连接失败，请确认", e);
            }
        }

        public IEnumerable<TValue> Read<TValue>(DataAddress address)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");

            var result = new List<TValue>();
            var valueType = typeof(TValue);

            try
            {
                var modbusAddress = ModbusAddress.Parse(address);

                switch (address.Type)
                {
                    case DataAddressType.Boolean:
                        {
                            var values = new bool[] { };

                            if (modbusAddress.IOType == IOType.DO) values = master.ReadCoils(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);
                            if (modbusAddress.IOType == IOType.DI) values = master.ReadInputs(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);

                            values.ToList().ForEach(e =>
                            {
                                result.Add((TValue)Convert.ChangeType(e, valueType));
                            });
                        }
                        break;
                    case DataAddressType.Short:
                        {
                            List<byte> datas = GetBytes(address);

                            for (int i = 0; i <= address.Offset; i++)
                            {
                                var bytes = datas.Skip(i * 2).Take(2).ToArray();
                                var value = (TValue)Convert.ChangeType(BitConverter.ToInt16(bytes, 0), valueType);

                                result.Add(value);
                            }
                        }
                        break;
                    case DataAddressType.Ushort:
                        {
                            var values = new ushort[] { };

                            if (modbusAddress.IOType == IOType.AO) values = master.ReadHoldingRegisters(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);
                            if (modbusAddress.IOType == IOType.AI) values = master.ReadInputRegisters(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);

                            values.ToList().ForEach(e =>
                            {
                                result.Add((TValue)Convert.ChangeType(e, valueType));
                            });
                        }
                        break;
                    case DataAddressType.Int:
                        {
                            List<byte> datas = GetBytes(address);

                            for (int i = 0; i <= address.Offset; i++)
                            {
                                var bytes = datas.Skip(i * 4).Take(4).ToArray();
                                var value = (TValue)Convert.ChangeType(BitConverter.ToInt32(bytes, 0), valueType);

                                result.Add(value);
                            }
                        }
                        break;
                    case DataAddressType.Float:
                        {
                            List<byte> datas = GetBytes(address);

                            for (int i = 0; i <= address.Offset; i++)
                            {
                                var bytes = datas.Skip(i * 4).Take(4).ToArray();
                                var value = (TValue)Convert.ChangeType(BitConverter.ToSingle(bytes, 0), valueType);

                                result.Add(value);
                            }
                        }
                        break;
                    case DataAddressType.String:
                        {
                            List<byte> datas = GetBytes(address, true);

                            var value = (TValue)Convert.ChangeType(Encoding.ASCII.GetString(datas.ToArray()).TrimEnd('\0'), valueType);

                            result.Add(value);
                        }
                        break;

                    default: throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                Reconnect();

                throw new ApplicationException($"读取失败，地址：{address }", e);
            }

            return result;
        }

        public void Write<TValue>(DataAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            var valueType = typeof(TValue);

            try
            {
                var modbusAddress = ModbusAddress.Parse(address);

                switch (address.Type)
                {
                    case DataAddressType.Boolean:
                        {
                            var datas = values.Select(e => (bool)Convert.ChangeType(e, valueType)).ToArray();

                            if (modbusAddress.IOType == IOType.DO) master.WriteMultipleCoils(modbusAddress.SlaveNumber, modbusAddress.StartAddress, datas);
                        }
                        break;
                    case DataAddressType.Short:
                        {
                            var datas = new List<ushort>();

                            values.ToList().ForEach(e =>
                            {
                                var value = (short)Convert.ChangeType(e, valueType);
                                var bytes = BitConverter.GetBytes(value);

                                datas.Add(BitConverter.ToUInt16(bytes.Reverse().ToArray(), 0));
                            });

                            Write(address, datas);
                        }
                        break;
                    case DataAddressType.Ushort:
                        {
                            var datas = values.Select(e => (ushort)Convert.ChangeType(e, valueType)).ToArray();

                            if (modbusAddress.IOType == IOType.AO) master.WriteMultipleRegisters(modbusAddress.SlaveNumber, modbusAddress.StartAddress, datas);
                        }
                        break;
                    case DataAddressType.Int:
                        {
                            var datas = new List<ushort>();

                            values.ToList().ForEach(e =>
                            {
                                var value = (int)Convert.ChangeType(e, valueType);
                                var bytes = BitConverter.GetBytes(value);

                                datas.Add(BitConverter.ToUInt16(new byte[] {
                                    bytes[1],
                                    bytes[0]
                                }, 0));
                                datas.Add(BitConverter.ToUInt16(new byte[] {
                                    bytes[3],
                                    bytes[2]
                                }, 0));
                            });

                            Write(address, datas);
                        }
                        break;
                    case DataAddressType.Float:
                        {
                            var datas = new List<ushort>();

                            values.ToList().ForEach(e =>
                            {
                                var value = (float)Convert.ChangeType(e, valueType);
                                var bytes = BitConverter.GetBytes(value);

                                datas.Add(BitConverter.ToUInt16(new byte[] {
                                    bytes[1],
                                    bytes[0]
                                }, 0));
                                datas.Add(BitConverter.ToUInt16(new byte[] {
                                    bytes[3],
                                    bytes[2]
                                }, 0));
                            });

                            Write(address, datas);
                        }
                        break;
                    case DataAddressType.String:
                        {
                            var datas = new List<ushort>();
                            var value = (string)Convert.ChangeType(values.Single(), valueType);
                            var bytes = new List<byte>(Encoding.ASCII.GetBytes(value));
                            var count = ((int)Math.Ceiling((address.Offset + 1) / 2d)) * 2 - bytes.Count;
                            while (count-- > 0) bytes.Add(0x00);

                            for (int i = 0; i < modbusAddress.Length; i++)
                            {
                                datas.Add(BitConverter.ToUInt16(bytes.Skip(i * 2).Take(2).Reverse().ToArray(), 0));
                            }

                            Write(address, datas);
                        }
                        break;

                    default: throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                Reconnect();

                throw new ApplicationException($"写入失败，地址：{address }", e);
            }
        }

        public void Close()
        {
            master?.Dispose();

            connectionStateChangedEventManager.StopMonitor(ConnectionStateChanged, this);
        }

        private List<byte> GetBytes(DataAddress address, bool isRam = false)
        {
            var result = new List<byte>();
            var modbusAddress = ModbusAddress.Parse(address);
            var values = new ushort[] { };

            if (modbusAddress.IOType == IOType.AO) values = master.ReadHoldingRegisters(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);
            if (modbusAddress.IOType == IOType.AI) values = master.ReadInputRegisters(modbusAddress.SlaveNumber, modbusAddress.StartAddress, modbusAddress.Length);

            values.ToList().ForEach(e =>
            {
                if (isRam)
                {
                    result.AddRange(BitConverter.GetBytes(e).Reverse());
                }
                else
                {
                    result.AddRange(BitConverter.GetBytes(e));
                }
            });

            return result;
        }

        private void Reconnect()
        {
            Close();

            Initialize();
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private IModbusMaster master;
        private readonly TcpClientComPortConfigInfo tcpClientComPortConfigInfo;
        private readonly SerialPortComPortConfigInfo serialPortComPortConfigInfo;
        private readonly ModbusTransportMode plcModbusTransportMode;
    }
}
