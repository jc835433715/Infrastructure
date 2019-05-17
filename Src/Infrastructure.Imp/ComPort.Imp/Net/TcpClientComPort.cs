
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Infrastructure.ComPort.Imp.Net
{
    public class TcpClientComPort : IComPort, IComPortCloneable
    {
        public TcpClientComPort(TcpClientComPortConfigInfo configInfo, ILoggerFactory loggerFactory = null)
        {
            this.configInfo = configInfo;
            this.tcpClient = null;
            this.loggerFactory = loggerFactory;
            this.Name = configInfo.Name;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(configInfo.Name, loggerFactory);
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public string Name { get; private set; }

        public bool IsConnected => tcpClient != null && tcpClient.Connected;

        public int BytesToRead => tcpClient.Client.Available;

        public void Open(bool isAsync = true)
        {
            loggerFactory?.GetLogger(Name).Info("打开通讯端口");

            tcpClient = CreateTcpClient();

            if (isAsync)
            {
                ConnectAsync();
            }
            else
            {
                ConnectSync();
            }

            connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已打开通讯端口");
        }

        private void ConnectSync()
        {
            try
            {
                tcpClient.Connect(configInfo.RemoteIPAddress, configInfo.RemotePort);

                KeepAliveHelper.SetKeepAlive(tcpClient.Client, 1000, 1000);
            }
            catch (Exception e)
            {
                var ex = new ApplicationException("打开通讯端口失败", e);

                loggerFactory?.GetLogger(Name).Error(ex.ToString());

                throw ex;
            }
        }

        private void ConnectAsync()
        {
            try
            {
                tcpClient.BeginConnect(configInfo.RemoteIPAddress, configInfo.RemotePort, ar =>
                {
                    var tcpClient = ar.AsyncState as TcpClient;

                    try
                    {
                        tcpClient.EndConnect(ar);

                        KeepAliveHelper.SetKeepAlive(tcpClient.Client, 1000, 1000);
                    }
                    catch (Exception e)
                    {
                        loggerFactory?.GetLogger(Name).Error(e.ToString());
                    }
                }, tcpClient);
            }
            catch (Exception e)
            {
                loggerFactory?.GetLogger(Name).Error(e.ToString());
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            loggerFactory?.GetLogger(Name).Info("写入数据" + Environment.NewLine +
                      "{0}：{1}" + Environment.NewLine +
                      "{2}：{3}" + Environment.NewLine +
                      "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

            tcpClient.GetStream().Write(buffer, offset, count);

            loggerFactory?.GetLogger(Name).Info("已写入数据");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var result = 0;

            loggerFactory?.GetLogger(Name).Info("读取数据" + Environment.NewLine +
            "{0}：{1}" + Environment.NewLine +
            "{2}：{3}" + Environment.NewLine +
            "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

            result = tcpClient.GetStream().Read(buffer, offset, count);

            loggerFactory?.GetLogger(Name).Info("已读取数据" + Environment.NewLine +
            "{0}：{1}" + Environment.NewLine +
            "{2}：{3}", nameof(buffer), ConvertHelper.ToString(buffer.Take(result).ToArray()), nameof(result), result);


            return result;
        }

        public void Close()
        {
            loggerFactory?.GetLogger(Name).Info("关闭通讯端口");

            tcpClient?.Close();

            connectionStateChangedEventManager.StopMonitor(ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已关闭通讯端口");
        }

        public void Dispose()
        {
            this.Close();
        }

        public IComPort Clone()
        {
            this.configInfo.LocalPort += 1;

            return new TcpClientComPort(this.configInfo, this.loggerFactory);
        }

        private TcpClient CreateTcpClient()
        {
            TcpClient result = null;

            if (string.IsNullOrEmpty(configInfo.LocalIPAddress))
            {
                result = new TcpClient()
                {
                    ReceiveTimeout = configInfo.ReceiveTimeout,
                    SendTimeout = configInfo.SendTimeout
                };
            }
            else
            {
                result = new TcpClient(new IPEndPoint(IPAddress.Parse(configInfo.LocalIPAddress), configInfo.LocalPort))
                {
                    ReceiveTimeout = configInfo.ReceiveTimeout,
                    SendTimeout = configInfo.SendTimeout
                };
            }

            return result;
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private TcpClientComPortConfigInfo configInfo;
        private TcpClient tcpClient;
        private ILoggerFactory loggerFactory;
    }
}
