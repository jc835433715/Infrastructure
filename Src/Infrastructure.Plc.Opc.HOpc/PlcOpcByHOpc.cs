using Hylasoft.Opc.Da;
using Hylasoft.Opc.Ua;
using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Ua;
using Infrastructure.ComPort.Interface;

namespace Infrastructure.Plc.Opc
{
    public class PlcOpcByHOpc : IPlcOpc
    {
        public PlcOpcByHOpc(string uriString, PlcOpcClientOptions plcOpcClientOptions = null)
        {
            this.uriString = uriString;
            this.plcOpcClientOptions = plcOpcClientOptions;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(PlcOpcByHOpc));
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Initialize()
        {
            try
            {
                if (uriString.StartsWith("opc.tcp"))
                {
                    if (plcOpcClientOptions != null)
                    {
                        const string name = "NowFuture";
                        var options = new Hylasoft.Opc.Ua.UaClientOptions()
                        {
                            ApplicationName = name,
                            ConfigSectionName = name,
                            SessionName = name
                        };

                        if (!string.IsNullOrWhiteSpace(plcOpcClientOptions.UserName)
                            && !string.IsNullOrWhiteSpace(plcOpcClientOptions.Password))
                        {
                            options.UserIdentity = new UserIdentity(plcOpcClientOptions.UserName, plcOpcClientOptions.Password);
                        }

                        client = new UaClient(new Uri(uriString), options);
                    }
                    else
                    {
                        client = new UaClient(new Uri(uriString));
                    }
                }

                if (uriString.StartsWith("opcda"))
                {
                    client = new DaClient(new Uri(uriString));
                }

                client.Connect();

                connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs() { IsConnected = true });
            }
            catch (Exception e)
            {
                throw new ApplicationException("建立与PLC连接失败，请确认", e);
            }
        }

        public IEnumerable<TValue> Read<TValue>(PlcAddress address)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");

            var result = new List<TValue>();

            try
            {
                var opcAddresses = address.Value.Split(PlcAddress.ValueSeparator);

                for (int index = 0; index < opcAddresses.Length; index++)
                {
                    var value = client.Read<TValue>(opcAddresses[index]).Value;

                    result.Add(value);
                }
            }
            catch (Exception e)
            {
                Reconnect();

                throw new ApplicationException($"读取失败，地址：{address }", e);
            }

            return result;
        }

        public void Write<TValue>(PlcAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(PlcAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            try
            {
                var opcAddresses = address.Value.Split(PlcAddress.ValueSeparator);
                var allValue = values.ToArray();

                for (int index = 0; index < opcAddresses.Length; index++)
                {
                    client.Write<TValue>(opcAddresses[index], allValue[index]);
                }
            }
            catch (Exception e)
            {
                Reconnect();

                throw new ApplicationException($"写入失败，地址：{address }", e);
            }
        }

        public void Monitor<TValue>(PlcAddress address, Action<TValue, Action> callback)
        {
            client.Monitor<TValue>(address.Value.Split(PlcAddress.ValueSeparator).Single(), callback);
        }

        public IEnumerable<PlcAddress> Explore(PlcAddress address)
        {
            IEnumerable<Hylasoft.Opc.Common.Node> nodes = client.ExploreFolder(address.Value.Split(PlcAddress.ValueSeparator).Single());

            return nodes.Select(e => new PlcAddress() { Value = e.Tag });
        }

        public void Close()
        {
            client?.Dispose();

            connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs() { IsConnected = false });
        }

        private void Reconnect()
        {
            Close();

            Initialize();
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private dynamic client;
        private readonly string uriString;
        private readonly PlcOpcClientOptions plcOpcClientOptions;
    }
}
