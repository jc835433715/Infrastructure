using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Infrastructure.ComPort.Imp.Net
{
    public class TcpListenerComPort : IComPort, INotifyTcpClientAccepted
    {
        public TcpListenerComPort(TcpListenerComPortConfigInfo configInfo, TcpListenerComPortType type, ILoggerFactory loggerFactory = null)
        {
            this.configInfo = configInfo;
            this.type = type;
            this.tcpClientInfoDictionary = new Dictionary<string, TcpClientInfo>();
            this.tcpListener = null;
            this.loggerFactory = loggerFactory;
            this.Name = configInfo.Name;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(Name, loggerFactory);
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public event EventHandler<TcpClientAcceptedEventArgs> TcpClientAccepted;

        public IEnumerable<TcpClientInfo> TcpClientInfos
        {
            get
            {
                return tcpClientInfoDictionary.Values;
            }
        }

        public string Name { get; private set; }

        public bool IsConnected
        {
            get
            {
                var result = false;

                if (type == TcpListenerComPortType.One2One)
                {
                    if (tcpClientInfoDictionary.Any())
                    {
                        result = tcpClientInfoDictionary.Single().Value.ComPort.IsConnected;
                    }
                }
                else
                {
                    return connectionStateChangedEventManager.LastConnectionStateChangedEventArgs.IsConnected;
                }

                return result;
            }
        }

        public int BytesToRead
        {
            get
            {
                var result = 0;

                if (type == TcpListenerComPortType.One2One)
                {
                    if (tcpClientInfoDictionary.Any())
                    {
                        result = tcpClientInfoDictionary.Single().Value.ComPort.BytesToRead;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }

                return result;
            }
        }

        public void Open(bool isAsync = true)
        {
            if (!isAsync) throw new InvalidOperationException($"参数有误，{nameof(isAsync)}为{isAsync }");

            loggerFactory?.GetLogger(Name).Info("打开通讯端口");

            tcpListener = new TcpListener(IPAddress.Parse(configInfo.LocalIPAddress), configInfo.LocalPort);

            tcpListener.Start();

            AcceptTcpClientAsync();

            connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs { IsConnected = true });

            loggerFactory?.GetLogger(Name).Info("已打开通讯端口");
        }

        private void AcceptTcpClientAsync()
        {
            try
            {
                tcpListener.BeginAcceptTcpClient(ar =>
                {
                    var tcpListener = ar.AsyncState as TcpListener;

                    try
                    {
                        TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
                        var ipAddress = string.Empty;

                        tcpClient.ReceiveTimeout = configInfo.ReceiveTimeout;
                        tcpClient.SendTimeout = configInfo.SendTimeout;

                        KeepAliveHelper.SetKeepAlive(tcpClient.Client, 1000, 1000);

                        ipAddress = tcpClient.Client.RemoteEndPoint.ToString().Split(':').First();

                        if (!tcpClientInfoDictionary.ContainsKey(ipAddress))
                        {
                            var tcpClientInfo = new TcpClientInfo()
                            {
                                IPAddress = ipAddress,
                                ComPort = new TcpClientWapper(tcpClient, loggerFactory)
                            };

                            tcpClientInfoDictionary.Add(ipAddress, tcpClientInfo);
                        }

                        (tcpClientInfoDictionary[ipAddress].ComPort as TcpClientWapper).TcpClient = tcpClient;

                        OnTcpClientAccepted(this, new TcpClientAcceptedEventArgs()
                        {
                            TcpClientInfo = tcpClientInfoDictionary[ipAddress]
                        });

                        if (type == TcpListenerComPortType.One2One)
                        {
                            connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, tcpClient);
                        }
                    }
                    catch (Exception e)
                    {
                        loggerFactory?.GetLogger(Name).Error(e.ToString());
                    }

                    AcceptTcpClientAsync();
                }, tcpListener);
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

            if (type == TcpListenerComPortType.One2One)
            {
                tcpClientInfoDictionary.Single().Value.ComPort.Write(buffer, offset, count);
            }
            else
            {
                throw new InvalidOperationException();
            }

            loggerFactory?.GetLogger(Name).Info("已写入数据");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            loggerFactory?.GetLogger(Name).Info("读取数据" + Environment.NewLine +
                 "{0}：{1}" + Environment.NewLine +
                 "{2}：{3}" + Environment.NewLine +
                 "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

            var result = 0;

            if (type == TcpListenerComPortType.One2One)
            {
                result = tcpClientInfoDictionary.Single().Value.ComPort.Read(buffer, offset, count);
            }
            else
            {
                throw new InvalidOperationException();
            }

            loggerFactory?.GetLogger(Name).Info("已读取数据" + Environment.NewLine +
                "{0}：{1}" + Environment.NewLine +
                "{2}：{3}", nameof(buffer), ConvertHelper.ToString(buffer.Take(result).ToArray()), nameof(result), result);

            return result;
        }

        public void Close()
        {
            loggerFactory?.GetLogger(Name).Info("关闭通讯端口");

            tcpClientInfoDictionary.Values.ToList().ForEach(e => e.ComPort.Close());

            tcpListener?.Stop();

            connectionStateChangedEventManager.StopMonitor(  ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已关闭通讯端口");
        }


        public void Dispose()
        {
            this.Close();
        }

        private void OnTcpClientAccepted(object sender, TcpClientAcceptedEventArgs e)
        {
            var handler = TcpClientAccepted;

            if (handler != null)
            {

                foreach (var del in handler.GetInvocationList())
                {
                    try
                    {
                        del.DynamicInvoke(sender, e);
                    }
                    catch (Exception ex)
                    {
                        loggerFactory?.GetLogger(Name).Error(ex.ToString());
                    }
                }
            }
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private TcpListenerComPortConfigInfo configInfo;
        private TcpListenerComPortType type;
        private Dictionary<string, TcpClientInfo> tcpClientInfoDictionary;
        private TcpListener tcpListener;
        private ILoggerFactory loggerFactory;
    }
}
