using Infrastructure.Common.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 连接状态改变事件管理器
    /// </summary>
    public class ConnectionStateChangedEventManager
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="connectionName">连接名</param>
        /// <param name="loggerFactory">日志工厂</param>
        public ConnectionStateChangedEventManager(string connectionName,
            ILoggerFactory loggerFactory = null)
        {
            this.LastConnectionStateChangedEventArgs = new ConnectionStateChangedEventArgs();
            this.ConnectionName = connectionName;
            this.loggerFactory = loggerFactory;
            this.shoudStop = false;
        }

        /// <summary>
        ///连接名
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// 连接转态改变事件参数
        /// </summary>
        public ConnectionStateChangedEventArgs LastConnectionStateChangedEventArgs { get; set; }

        /// <summary>
        /// 启动监视
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="comPort">通讯端口</param>
        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, IComPort comPort)
        {
            StartMonitor(() => comPort?.Write(new byte[] { }, 0, 0), comPort, connectionStateChanged);
        }

        /// <summary>
        /// 启动监视
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="sender">发送者</param>
        /// <param name="socket">Socket</param>
        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender, Socket socket)
        {
            StartMonitor(() => socket?.Send(new byte[] { }, 0, 0), sender , connectionStateChanged);
        }

        /// <summary>
        /// 启动监视
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="sender">发送者</param>
        /// <param name="tcpClient">TcpClient</param>
        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender, TcpClient tcpClient)
        {
            StartMonitor(() => tcpClient?.GetStream().Write(new byte[] { }, 0, 0), sender , connectionStateChanged);
        }

        /// <summary>
        /// 启动监视
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="sender">发送者</param>
        /// <param name="serialPort">SerialPort</param>
        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender, System.IO.Ports.SerialPort serialPort)
        {
            StartMonitor(() => serialPort?.Write(new byte[] { }, 0, 0), sender , connectionStateChanged);
        }

        /// <summary>
        /// 启动监视
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="sender">发送者</param>
        public void StopMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender)
        {
            shoudStop = true;

            OnConnectionStateChanged(connectionStateChanged, sender, new ConnectionStateChangedEventArgs { IsConnected = false });
        }

        /// <summary>
        /// 触发连接状态改变事件
        /// </summary>
        /// <param name="connectionStateChanged">连接状态改变事件事件委托</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        public void OnConnectionStateChanged(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender, ConnectionStateChangedEventArgs e)
        {
            var handler = connectionStateChanged;

            if (handler != null)
            {
                if (e.IsConnected ^ LastConnectionStateChangedEventArgs.IsConnected)
                {
                    if (!string.IsNullOrEmpty(ConnectionName)) e.Name = ConnectionName;

                    foreach (var del in handler.GetInvocationList())
                    {
                        try
                        {
                            del.DynamicInvoke(sender, e);
                        }
                        catch (Exception ex)
                        {
                            loggerFactory?.GetLogger(e.Name).Error(ex.ToString());
                        }
                    }
                }
            }

            LastConnectionStateChangedEventArgs = e;
        }

        private void StartMonitor(Action Test, object sender, EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged)
        {
            shoudStop = false;

            ThreadHelper.StartThread(() =>
            {
                while (!shoudStop)
                {
                    try
                    {
                        Test();

                        OnConnectionStateChanged(connectionStateChanged, sender, new ConnectionStateChangedEventArgs { IsConnected = true });
                    }
                    catch
                    {
                        OnConnectionStateChanged(connectionStateChanged, sender, new ConnectionStateChangedEventArgs { IsConnected = false });
                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        private bool shoudStop;
        private readonly ILoggerFactory loggerFactory;
    }
}
