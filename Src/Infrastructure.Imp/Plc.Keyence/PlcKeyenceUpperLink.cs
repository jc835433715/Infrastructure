using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.Utils;

namespace Infrastructure.Plc.Keyence
{
    public class PlcKeyenceUpperLink : IPlc
    {
        public PlcKeyenceUpperLink(IComPort comPort, int readTimeoutSeconds = 3)
        {
            this.comPort = comPort;
            this.readTimeoutSeconds = readTimeoutSeconds;
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
                if (!comPort.IsConnected) comPort.Open(false);

                this.ReadSingle<short>(new PlcAddress()
                {
                    Name = "基恩士通讯探测",
                    Type = PlcAddressType.Short,
                    Offset = 0,
                    Value = "DM0"
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException("建立与PLC连接失败，请确认", e);
            }
        }

        public IEnumerable<TValue> Read<TValue>(PlcAddress address)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");

            var command = ProtocolUpperLinkCommand.GetReadCommand(address);
            byte[] response = { };

            using (new StopwatchWrapper($"读取PLC"))
            {
                response = Send(command);
            }

            if (!response.Any())
            {
                Reconnect();

                throw new ApplicationException($"读取失败，地址：{address }");
            }

            return GetValues<TValue>(address, response);
        }

        public void Write<TValue>(PlcAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            var command = ProtocolUpperLinkCommand.GetWriteCommand(address, values);
            byte[] response = { };

            using (new StopwatchWrapper($"写入PLC"))
            {
                response = Send(command);
            }

            if (!response.Any() || !GetString(response).Contains("OK"))
            {
                Reconnect();

                throw new ApplicationException($"写入失败，地址：{address }");
            }
        }

        private string GetString(byte[] response)
        {
            return Encoding.ASCII.GetString(response);
        }

        private TValue[] GetValues<TValue>(PlcAddress address, byte[] response)
        {
            var result = new List<TValue>();
            var valueType = typeof(TValue);

            if (response.Any())
            {
                var datas = GetString(response).TrimEnd('\n').TrimEnd('\r').Split(' ').ToList();

                if (datas.Any())
                {
                    switch (address.Type)
                    {
                        case PlcAddressType.Boolean:
                            {
                                var value = (TValue)Convert.ChangeType(datas.Single() == "1", valueType);

                                result.Add(value);
                            }
                            break;
                        case PlcAddressType.Short:
                            {
                                datas.ForEach(e =>
                                {
                                    var value = (TValue)Convert.ChangeType(short.Parse(e), valueType);

                                    result.Add(value);
                                });
                            }
                            break;
                        case PlcAddressType.Ushort:
                            {
                                datas.ForEach(e =>
                                {
                                    var value = (TValue)Convert.ChangeType(ushort.Parse(e), valueType);

                                    result.Add(value);
                                });
                            }
                            break;
                        case PlcAddressType.Int:
                            {
                                datas.ForEach(e =>
                                {
                                    var value = (TValue)Convert.ChangeType(int.Parse(e), valueType);

                                    result.Add(value);
                                });
                            }
                            break;
                        case PlcAddressType.Float:
                            {
                                var bytes = new List<byte>();

                                datas.ForEach(e =>
                                {
                                    bytes.Add(Convert.ToByte(e.Substring(0, 2), 16));
                                    bytes.Add(Convert.ToByte(e.Substring(2, 2), 16));
                                });

                                for (int index = 0; index <= address.Offset; index++)
                                {
                                    var temp = bytes.Skip(index * 4).Take(4).ToArray();

                                    var value = (TValue)Convert.ChangeType(BitConverter.ToSingle(new byte[]
                                    {
                                    temp[1],
                                    temp[0],
                                    temp[3],
                                    temp[2]
                                    }, 0), valueType);

                                    result.Add(value);
                                }
                            }
                            break;
                        case PlcAddressType.String:
                            {
                                var value = (TValue)Convert.ChangeType(string.Empty, valueType);
                                var bytes = new List<byte>();

                                datas.ForEach(e =>
                                {
                                    bytes.Add(Convert.ToByte(e.Substring(0, 2), 16));
                                    bytes.Add(Convert.ToByte(e.Substring(2, 2), 16));
                                });

                                value = (TValue)Convert.ChangeType(Encoding.ASCII.GetString(bytes.ToArray()).TrimEnd('\0'), valueType);

                                result.Add(value);
                            }
                            break;

                        default: throw new NotImplementedException();
                    }
                }
            }

            return result.ToArray();
        }

        public void Close()
        {
            comPort.Close();
        }

        private byte[] Send(byte[] command)
        {
            var result = new byte[] { };

            lock (syncObj)
            {
                comPort.Write(command, 0, command.Length);

                result = Read();
            }

            return result;
        }

        private byte[] Read()
        {
            var result = new List<byte>();
            var buffer = new byte[1024];
            var dt = DateTime.Now.AddSeconds(readTimeoutSeconds);

            do
            {
                if (comPort.BytesToRead != 0)
                {
                    var readBufferCount = comPort.Read(buffer, 0, buffer.Length);

                    if (readBufferCount > 0)
                    {
                        result.AddRange(buffer.Take(readBufferCount));
                    }
                }

                if (result.Count > 2
                    && (result[result.Count - 2] == 0x0D && result[result.Count - 1] == 0x0A))
                {
                    break;
                }
                else
                {
                    ThreadHelper.Sleep(1);
                }
            } while (DateTime.Now <= dt);

            return result.ToArray();
        }

        private void Reconnect()
        {
            comPort.Close();
            comPort.Open();

            ThreadHelper.Sleep(500);
        }

        private readonly object syncObj = new object();
        private readonly IComPort comPort;
        private readonly int readTimeoutSeconds;
        private readonly ConnectionStateChangedEventManager connectionStateChangedEventManager;
    }
}
