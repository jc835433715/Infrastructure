using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Opc.Interface;
using Infrastructure.Plc.Interface;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Opc.OpcUaHelper
{
    public class OpcClientByOpcUaHelper : IOpcClient
    {
        public OpcClientByOpcUaHelper(string uriString, OpcClientOptions opcClientOptions = null)
        {
            this.uriString = uriString;
            this.opcClientOptions = opcClientOptions;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(OpcClientByOpcUaHelper));
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Initialize()
        {
            try
            {
                client = new OpcUaClient() { OpcUaName = "NowFuture" };

                if (opcClientOptions != null
                    && !string.IsNullOrWhiteSpace(opcClientOptions.UserName)
                    && !string.IsNullOrWhiteSpace(opcClientOptions.Password))
                {
                    client.UserIdentity = new UserIdentity(opcClientOptions.UserName, opcClientOptions.Password);
                }

                client.ConnectServer(uriString);

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

                result = client.ReadNodes<TValue>(opcAddresses);
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
                var allValue = values.Select(e => (object)e).ToArray();

                client.WriteNodes(opcAddresses, allValue);
            }
            catch (Exception e)
            {
                Reconnect();

                throw new ApplicationException($"写入失败，地址：{address }", e);
            }
        }

        public void Monitor<TValue>(DataAddress address, Action<TValue, Action> callback)
        {
            client.AddSubscription(address.ToString(), address.Value.Split(DataAddress.ValueSeparator).Single(), (k, mi, ea) =>
            {
                MonitoredItemNotification notification = ea.NotificationValue as MonitoredItemNotification;

                try
                {
                    callback((TValue)notification.Value.WrappedValue.Value, () => client.RemoveSubscription(k));
                }
                catch (Exception e)
                {
                    Reconnect();

                    throw new ApplicationException($"回调失败，地址：{address }", e);
                }
            });
        }

        public IEnumerable<DataAddress> Explore(DataAddress address)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            client?.Disconnect();

            connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs() { IsConnected = false });
        }

        private void Reconnect()
        {
            Close();

            Initialize();
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private OpcUaClient client;
        private readonly string uriString;
        private readonly OpcClientOptions opcClientOptions;
    }
}
