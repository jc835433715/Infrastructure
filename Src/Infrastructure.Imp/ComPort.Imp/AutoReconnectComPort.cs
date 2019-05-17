using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using System;

namespace Infrastructure.ComPort.Imp
{
    public class AutoReconnectComPort : IComPort
    {
        public AutoReconnectComPort(IComPort comPort, ILoggerFactory loggerFactory = null)
        {
            this.comPort = comPort;
            this.loggerFactory = loggerFactory;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(comPort.Name, loggerFactory);
            this.syncObj = new object();

            this.ConnectionStateChanged += (s, e) =>
            {
                if (!e.IsConnected)
                {
                    Reconnect();
                };
            };
        }

        public bool IsConnected
        {
            get
            {
                var result = false;

                try
                {
                    result = comPort.IsConnected;
                }
                catch (Exception e)
                {
                    loggerFactory?.GetLogger(Name).Error(e.ToString());

                    Reconnect();
                }

                return result;

            }
        }

        public string Name => comPort.Name;

        public int BytesToRead
        {
            get
            {
                var result = 0;

                try
                {
                    result = comPort.BytesToRead;
                }
                catch (Exception e)
                {
                    loggerFactory?.GetLogger(Name).Error(e.ToString());

                    Reconnect();
                }

                return result;
            }
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public void Open(bool isAsync = true)
        {
            comPort.Open(isAsync);

            connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, comPort);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;

            try
            {
                result = comPort.Read(buffer, offset, count);
            }
            catch (Exception e)
            {
                loggerFactory?.GetLogger(Name).Error(e.ToString());

                Reconnect();
            }

            return result;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                comPort.Write(buffer, offset, count);
            }
            catch (Exception e)
            {
                loggerFactory?.GetLogger(Name).Error(e.ToString());

                Reconnect();
            }
        }

        public void Close()
        {
            comPort.Close();

            connectionStateChangedEventManager.StopMonitor(ConnectionStateChanged, this);
        }

        public void Dispose()
        {
            Close();
        }

        private void Reconnect()
        {
            lock (syncObj)
            {
                try
                {
                    comPort?.Close();

                    comPort.Open();
                }
                catch (Exception e)
                {
                    loggerFactory?.GetLogger(Name).Error(e.ToString());
                }
            }
        }

        private object syncObj;
        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private IComPort comPort;
        private readonly ILoggerFactory loggerFactory;
    }
}
