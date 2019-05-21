using HslCommunication;
using HslCommunication.Profinet.Melsec;
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Plc.Mitsubishi
{
    public class PlcMelsecMcBinary : IPlc
    {

        public PlcMelsecMcBinary(string ip, ushort port)
        {
            this.ip = ip;
            this.port = port;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(PlcMelsecMcBinary));
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public void Initialize()
        {
            this.melsecMcNet = new MelsecMcNet()
            {
                IpAddress = ip,
                Port = port
            };

            if (!melsecMcNet.ConnectServer().IsSuccess) throw new ApplicationException("建立与PLC连接失败，请确认");

            connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, melsecMcNet.AlienSession.Socket);
        }

        public IEnumerable<TValue> Read<TValue>(DataAddress address)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");

            var result = new List<TValue>();
            var valueType = typeof(TValue);

            switch (address.Type)
            {
                case DataAddressType.Boolean:
                    {
                        var operateResult = melsecMcNet.ReadBool(address.Value);

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        var value = (TValue)Convert.ChangeType(operateResult.Content, valueType);

                        result.Add(value);
                    }
                    break;
                case DataAddressType.Short:
                    {
                        var operateResult = melsecMcNet.ReadInt16(address.Value, (ushort)(address.Offset + 1));

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        operateResult.Content.ToList().ForEach(e =>
                        {
                            var value = (TValue)Convert.ChangeType(e, valueType);

                            result.Add(value);
                        });
                    }
                    break;
                case DataAddressType.Ushort:
                    {
                        var operateResult = melsecMcNet.ReadUInt16(address.Value, (ushort)(address.Offset + 1));

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        operateResult.Content.ToList().ForEach(e =>
                        {
                            var value = (TValue)Convert.ChangeType(e, valueType);

                            result.Add(value);
                        });
                    }
                    break;
                case DataAddressType.Int:
                    {
                        var operateResult = melsecMcNet.ReadInt32(address.Value, (ushort)(address.Offset + 1));

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        operateResult.Content.ToList().ForEach(e =>
                        {
                            var value = (TValue)Convert.ChangeType(e, valueType);

                            result.Add(value);
                        });
                    }
                    break;
                case DataAddressType.Float:
                    {
                        var operateResult = melsecMcNet.ReadFloat(address.Value, (ushort)(address.Offset + 1));

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        operateResult.Content.ToList().ForEach(e =>
                        {
                            var value = (TValue)Convert.ChangeType(e, valueType);

                            result.Add(value);
                        });
                    }
                    break;
                case DataAddressType.String:
                    {
                        var operateResult = melsecMcNet.ReadString(address.Value, (ushort)(address.Offset + 1));

                        CheckResult(operateResult, $"读取失败，地址：{address }");

                        var value = (TValue)Convert.ChangeType(operateResult.Content, valueType);

                        result.Add(value);
                    }
                    break;

                default: throw new NotImplementedException();
            }

            return result;
        }

        public void Write<TValue>(DataAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            var valueType = typeof(TValue);
            OperateResult operateResult;

            switch (address.Type)
            {
                case DataAddressType.Boolean:
                    {
                        var value = (bool)Convert.ChangeType(values.Single(), valueType);

                        operateResult = melsecMcNet.Write(address.Value, value);
                    }
                    break;
                case DataAddressType.Short:
                    {
                        var list = new List<short>();

                        values.ToList().ForEach(e =>
                        {
                            var value = (short)Convert.ChangeType(e, valueType);

                            list.Add(value);
                        });

                        operateResult = melsecMcNet.Write(address.Value, list.ToArray());
                    }
                    break;
                case DataAddressType.Ushort:
                    {
                        var list = new List<ushort>();

                        values.ToList().ForEach(e =>
                        {
                            var value = (ushort)Convert.ChangeType(e, valueType);

                            list.Add(value);
                        });

                        operateResult = melsecMcNet.Write(address.Value, list.ToArray());
                    }
                    break;
                case DataAddressType.Int:
                    {
                        var list = new List<int>();

                        values.ToList().ForEach(e =>
                        {
                            var value = (int)Convert.ChangeType(e, valueType);

                            list.Add(value);
                        });

                        operateResult = melsecMcNet.Write(address.Value, list.ToArray());
                    }
                    break;
                case DataAddressType.Float:
                    {
                        var list = new List<float>();

                        values.ToList().ForEach(e =>
                        {
                            var value = (float)Convert.ChangeType(e, valueType);

                            list.Add(value);
                        });

                        operateResult = melsecMcNet.Write(address.Value, list.ToArray());
                    }
                    break;
                case DataAddressType.String:
                    {
                        var value = (string)Convert.ChangeType(values.Single(), valueType);

                        operateResult = melsecMcNet.Write(address.Value, value);
                    }
                    break;
                default: throw new NotImplementedException();
            }

            CheckResult(operateResult, $"写入失败，地址：{address }");
        }

        public void Close()
        {
            melsecMcNet?.ConnectClose();

            connectionStateChangedEventManager.StopMonitor( ConnectionStateChanged, this);

        }

        private void CheckResult(dynamic result, string message)
        {
            if (!result.IsSuccess)
            {
                Reconnect();

                throw new ApplicationException(message);
            }
        }

        private void Reconnect()
        {
            melsecMcNet?.ConnectClose();

            Initialize();
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private MelsecMcNet melsecMcNet;
        private readonly string ip;
        private readonly ushort port;
    }
}
