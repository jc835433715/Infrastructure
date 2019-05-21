using Hylasoft.Opc.Da;
using Hylasoft.Opc.Ua;
using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Opc.Ua;
using Infrastructure.ComPort.Interface;
using Infrastructure.Opc.Interface;

namespace Infrastructure.Opc.HOpc
{
    public class OpcClientByHOpc : IOpcClient
    {
        public OpcClientByHOpc(string uriString, OpcClientOptions opcClientOptions = null)
        {
            this.uriString = uriString;
            this.opcClientOptions = opcClientOptions;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(OpcClientByHOpc));
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Initialize()
        {
            try
            {
                if (uriString.StartsWith("opc.tcp"))
                {
                    if (opcClientOptions != null)
                    {
                        const string name = "NowFuture";
                        var options = new Hylasoft.Opc.Ua.UaClientOptions()
                        {
                            ApplicationName = name,
                            ConfigSectionName = name,
                            SessionName = name
                        };

                        if (!string.IsNullOrWhiteSpace(opcClientOptions.UserName)
                            && !string.IsNullOrWhiteSpace(opcClientOptions.Password))
                        {
                            options.UserIdentity = new UserIdentity(opcClientOptions.UserName, opcClientOptions.Password);
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

        public IEnumerable<TValue> Read<TValue>(DataAddress address)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");

            var result = new List<TValue>();

            try
            {
                var opcAddresses = address.Value.Split(DataAddress.ValueSeparator);

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

        public void Write<TValue>(DataAddress address, IEnumerable<TValue> values)
        {
            if (address.Equals(DataAddress.Empty)) throw new ApplicationException("地址为空");
            if (values == null || !values.Any()) throw new ApplicationException($"参数异常，values为null或空");

            try
            {
                var opcAddresses = address.Value.Split(DataAddress.ValueSeparator);
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

        public void Monitor<TValue>(DataAddress address, Action<TValue, Action> callback)
        {
            client.Monitor<TValue>(address.Value.Split(DataAddress.ValueSeparator).Single(), callback);
        }

        public IEnumerable<DataAddress> Explore(DataAddress address)
        {
            IEnumerable<Hylasoft.Opc.Common.Node> nodes = client.ExploreFolder(address.Value.Split(DataAddress.ValueSeparator).Single());

            return nodes.Select(e => new DataAddress() { Value = e.Tag });
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
        private readonly OpcClientOptions opcClientOptions;
    }
}
