using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;

namespace Infrastructure.ComPort.Imp
{
    class ConnectionStateChangedEventManager
    {
        public ConnectionStateChangedEventManager(string connectionName,
            ILoggerFactory loggerFactory = null)
        {
            this.LastConnectionStateChangedEventArgs = new ConnectionStateChangedEventArgs();
            this.ConnectionName = connectionName;
            this.loggerFactory = loggerFactory;
            this.shoudStop = false;
        }

        public string ConnectionName { get; set; }

        public ConnectionStateChangedEventArgs LastConnectionStateChangedEventArgs { get; set; }

        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, IComPort comPort)
        {
            StartMonitor(() => comPort?.Write(new byte[] { }, 0, 0), comPort, connectionStateChanged);
        }

        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, Socket socket)
        {
            StartMonitor(() => socket?.Send(new byte[] { }, 0, 0), socket, connectionStateChanged);
        }

        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, TcpClient tcpClient)
        {
            StartMonitor(() => tcpClient?.GetStream().Write(new byte[] { }, 0, 0), tcpClient, connectionStateChanged);
        }

        public void StartMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, System.IO.Ports.SerialPort serialPort)
        {
            StartMonitor(() => serialPort?.Write(new byte[] { }, 0, 0), serialPort, connectionStateChanged);
        }

        public void StopMonitor(EventHandler<ConnectionStateChangedEventArgs> connectionStateChanged, object sender)
        {
            shoudStop = true;

            OnConnectionStateChanged(connectionStateChanged, sender, new ConnectionStateChangedEventArgs { IsConnected = false });
        }

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
