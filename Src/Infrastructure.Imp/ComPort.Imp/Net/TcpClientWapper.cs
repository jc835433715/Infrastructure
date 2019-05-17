using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Infrastructure.ComPort.Imp.Net
{
    class TcpClientWapper : IComPort
    {
        public TcpClientWapper(TcpClient tcpClient, ILoggerFactory loggerFactory)
        {
            this.TcpClient = tcpClient;
            this.loggerFactory = loggerFactory;
            this.Name = tcpClient.Client.RemoteEndPoint.ToString().Split(':').First();
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(Name, loggerFactory);

            this.connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, this);
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public TcpClient TcpClient { get; set; }

        public string Name { get; private set; }

        public bool IsConnected
        {
            get { return TcpClient.Connected; }
        }

        public int BytesToRead
        {
            get
            {
                var result = 0;

                try
                {
                    result = TcpClient.Client.Available;
                }
                catch (Exception e)
                {
                    loggerFactory?.GetLogger(Name).Error(e.ToString());
                }

                return result;
            }
        }

        public void Open(bool isAsync = true)
        {
            throw new InvalidOperationException();
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                loggerFactory?.GetLogger(Name).Info("写入数据" + Environment.NewLine +
                           "{0}：{1}" + Environment.NewLine +
                           "{2}：{3}" + Environment.NewLine +
                           "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

                TcpClient.GetStream().Write(buffer, offset, count);

                loggerFactory?.GetLogger(Name).Info("已写入数据");
            }
            catch (Exception e)
            {
                loggerFactory?.GetLogger(Name).Error(e.ToString());
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var result = 0;

            try
            {
                loggerFactory?.GetLogger(Name).Info("读取数据" + Environment.NewLine +
                  "{0}：{1}" + Environment.NewLine +
                  "{2}：{3}" + Environment.NewLine +
                  "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

                result = TcpClient.GetStream().Read(buffer, offset, count);

                loggerFactory?.GetLogger(Name).Info("已读取数据" + Environment.NewLine +
                "{0}：{1}" + Environment.NewLine +
                "{2}：{3}", nameof(buffer), ConvertHelper.ToString(buffer.Take(result).ToArray()), nameof(result), result);
            }
            catch (Exception e)
            {
                loggerFactory?.GetLogger(Name).Error(e.ToString());
            }

            return result;
        }

        public void Close()
        {
            loggerFactory?.GetLogger(Name).Info("关闭通讯接口");

            TcpClient.Close();

            connectionStateChangedEventManager.StopMonitor(ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已关闭通讯接口");
        }

        public void Dispose()
        {
            this.Close();
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private ILoggerFactory loggerFactory;
    }
}
