using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Omron
{
    /// <summary>
    ///欧姆龙Fins协议实现
    /// 
    ///十六进制错误码|含义 
    ///00000000      |正常
    ///00000001      |头不是‘FINS’ (ASCII code)。
    ///00000002      |数据太长。
    ///00000003      |不支持的命令。
    ///00000020      |所有的连接被占用。
    ///00000021      |制定的节点已经连接。
    ///00000022      |未被指定的IP地址试图访问一个被保护的节点。
    ///00000023      |客户端FINS节点地址超范围。
    ///00000024      |相同的FINS节点地址已经被使用。
    ///00000025      |所有可用的节点地址都已使用。 
    /// </summary>
    public class PlcOmronFins : IPlc
    {
        public PlcOmronFins(IComPort comPort, int readTimeoutSeconds = 3)
        {
            this.comPort = comPort;
            this.readTimeoutSeconds = readTimeoutSeconds;
            this.pcNode = 0x00;
            this.plcNode = 0x00;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(string.Empty);

            this.comPort.ConnectionStateChanged += (s, e) =>
            {
                connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, s, e);
            };
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Initialize()
        {
            try
            {
                var command = ProtocolFinsCommand.GetHandshakeCommand(pcNode);
                var response = new byte[] { };
                
                if (!comPort.IsConnected) comPort.Open(false);

                response = Send(command);

                if (!response.Any() || GetErrorCode(response) != ErrorCodeOK)
                {
                    throw new ApplicationException($"握手失败，错误码：{GetErrorCode(response)}");
                }
                else
                {
                    pcNode = response[19];
                    plcNode = response[23];
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("建立与PLC连接失败，请确认", e);
            }
        }

        public IEnumerable<TValue> Read<TValue>(PlcAddress address)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");

            var command = ProtocolFinsCommand.GetReadCommand(address, pcNode, plcNode);
            byte[] response = { };

            using (new StopwatchWrapper($"读取PLC"))
            {
                response = Send(command);
            }

            if (!response.Any() || GetErrorCode(response) != ErrorCodeOK)
            {
                Reconnect();

                throw new ApplicationException($"读取失败，地址：{address }，错误码：{GetErrorCode(response)}");
            }

            return GetValues<TValue>(address, response);
        }

        public void Write<TValue>(PlcAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            var command = ProtocolFinsCommand.GetWriteCommand(address, values, pcNode, plcNode);
            byte[] response = { };

            using (new StopwatchWrapper($"写入PLC"))
            {
                response = Send(command);
            }

            if (!response.Any() || GetErrorCode(response) != ErrorCodeOK)
            {
                Reconnect();

                throw new ApplicationException($"写入失败，地址：{address }，错误码：{GetErrorCode(response)}");
            }
        }

        private TValue[] GetValues<TValue>(PlcAddress address, byte[] response)
        {
            var result = new List<TValue>();
            var valueType = typeof(TValue);

            if (response.Any() && GetErrorCode(response) == ErrorCodeOK)
            {
                var datas = response.Skip(30).ToArray();

                switch (address.Type)
                {
                    case PlcAddressType.Boolean:
                        {
                            var value = (TValue)Convert.ChangeType(datas.Single() != 0, valueType);

                            result.Add(value);
                        }
                        break;
                    case PlcAddressType.Short:
                        for (int index = 0; index <= address.Offset; index++)
                        {
                            var bytes = datas.Skip(index * 2).Take(2).ToArray();

                            var value = (TValue)Convert.ChangeType(BitConverter.ToInt16(new byte[]
                            {
                                    bytes[1],
                                    bytes[0]
                            }, 0), valueType);

                            result.Add(value);
                        }
                        break;
                    case PlcAddressType.Ushort:
                        for (int index = 0; index <= address.Offset; index++)
                        {
                            var bytes = datas.Skip(index * 2).Take(2).ToArray();

                            var value = (TValue)Convert.ChangeType(BitConverter.ToUInt16(new byte[]
                            {
                                    bytes[1],
                                    bytes[0]
                            }, 0), valueType);

                            result.Add(value);
                        }
                        break;
                    case PlcAddressType.Int:
                        for (int index = 0; index <= address.Offset; index++)
                        {
                            var bytes = datas.Skip(index * 4).Take(4).ToArray();

                            var value = (TValue)Convert.ChangeType(BitConverter.ToInt32(new byte[]
                            {
                                    bytes[1],
                                    bytes[0],
                                    bytes[3],
                                    bytes[2]
                            }, 0), valueType);

                            result.Add(value);
                        }
                        break;
                    case PlcAddressType.Float:
                        for (int index = 0; index <= address.Offset; index++)
                        {
                            var bytes = datas.Skip(index * 4).Take(4).ToArray();

                            var value = (TValue)Convert.ChangeType(BitConverter.ToSingle(new byte[]
                            {
                                    bytes[1],
                                    bytes[0],
                                    bytes[3],
                                    bytes[2]
                            }, 0), valueType);

                            result.Add(value);
                        }
                        break;
                    case PlcAddressType.String:
                        {
                            var value = (TValue)Convert.ChangeType(Encoding.ASCII.GetString(datas).TrimEnd('\0'), valueType);

                            result.Add(value);
                        }
                        break;
                    default: throw new NotImplementedException();
                }
            }

            return result.ToArray();
        }

        public void Close()
        {
            comPort.Close();
        }

        private string GetErrorCode(byte[] response)
        {
            var result = "FFFFFFFF";

            if (response.Length > 16)
            {
                result = string.Join(string.Empty, response.Skip(12).Take(4).Select(e => e.ToString("X2")).ToArray());
            }

            return result;
        }

        private byte[] Send(byte[] command)
        {
            var result = new List<byte>();
            var length = int.MaxValue;
            var buffer = new byte[] { };
            const int HeaderLengthBytesCount = 8;

            lock (syncObj)
            {
                comPort.Write(command, 0, command.Length);

                buffer = Read(HeaderLengthBytesCount);
                if (buffer.Any())
                {
                    var header = buffer.Take(4);
                    var dataLength = buffer.Skip(4).Take(4).ToArray();

                    if (Encoding.ASCII.GetString(header.ToArray()) == "FINS")
                    {
                        length = BitConverter.ToUInt16(new byte[] {
                            dataLength [3],
                            dataLength [2]
                        }, 0) + HeaderLengthBytesCount;

                        result.AddRange(buffer);
                    }
                }

                if (result.Any())
                {
                    buffer = Read(length - HeaderLengthBytesCount);
                    if (buffer.Any())
                    {
                        result.AddRange(buffer);
                    }
                    else
                    {
                        result.Clear();
                    }
                }
            }

            return result.ToArray();
        }

        private byte[] Read(int bytesToRead)
        {
            var result = new byte[] { };
            byte[] buffer = new byte[bytesToRead];
            int readBufferOffset = 0;
            var dt = DateTime.Now.AddSeconds(readTimeoutSeconds);

            do
            {
                if (comPort.BytesToRead != 0)
                {
                    int readBufferCount = comPort.Read(buffer, readBufferOffset, buffer.Length - readBufferOffset);

                    if (readBufferCount <= 0) break;

                    readBufferOffset += readBufferCount;
                }

                if (readBufferOffset == bytesToRead)
                {
                    result = buffer;

                    break;
                }
                else
                {
                    ThreadHelper.Sleep(1);
                }
            } while (DateTime.Now <= dt);

            return result;
        }

        private void Reconnect()
        {
            comPort.Close();
            comPort.Open();

            ThreadHelper.Sleep(500);

            Initialize();
        }

        private const string ErrorCodeOK = "00000000";
        private readonly object syncObj = new object();
        private byte plcNode;
        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private byte pcNode;
        private IComPort comPort;
        private int readTimeoutSeconds;
    }
}
