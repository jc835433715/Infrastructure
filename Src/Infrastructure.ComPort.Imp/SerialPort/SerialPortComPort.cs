using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Linq;

namespace Infrastructure.ComPort.Imp.SerialPort
{
    public class SerialPortComPort : IComPort, INotifyDataReceived
    {
        public SerialPortComPort(SerialPortComPortConfigInfo serialPortConfigInfo, ILoggerFactory loggerFactory = null)
        {
            this.serialPortConfigInfo = serialPortConfigInfo;
            this.serialPort = new System.IO.Ports.SerialPort(
                serialPortConfigInfo.PortName,
                serialPortConfigInfo.BaudRate,
                serialPortConfigInfo.Parity,
                serialPortConfigInfo.DataBits,
                serialPortConfigInfo.StopBits);
            this.serialPort.DataReceived += (s, e) => OnDataReceived(s, new DataReceivedEventArgs() { BytesToRead = serialPort.BytesToRead });
            this.loggerFactory = loggerFactory;
            this.Name = serialPortConfigInfo.Name;
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(Name, loggerFactory);
        }

        public string Name { get; private set; }

        public bool IsConnected => serialPort != null && serialPort.IsOpen;

        public int BytesToRead => serialPort.BytesToRead;

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void Open(bool isAsync = true)
        {
            if (!isAsync) throw new InvalidOperationException($"参数有误，{nameof(isAsync)}为{isAsync }");

            loggerFactory?.GetLogger(Name).Info("打开通讯端口");

            serialPort = new System.IO.Ports.SerialPort(
                 serialPortConfigInfo.PortName,
                 serialPortConfigInfo.BaudRate,
                 serialPortConfigInfo.Parity,
                 serialPortConfigInfo.DataBits,
                 serialPortConfigInfo.StopBits);
            serialPort.WriteTimeout = serialPortConfigInfo.SendTimeout;
            serialPort.ReadTimeout = serialPortConfigInfo.ReceiveTimeout;

            serialPort.DataReceived += (s, e) => OnDataReceived(s, new DataReceivedEventArgs() { BytesToRead = serialPort.BytesToRead });

            serialPort.Open();

            connectionStateChangedEventManager.StartMonitor(ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已打开通讯端口");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var result = 0;

            loggerFactory?.GetLogger(Name).Info("读取数据" + Environment.NewLine +
            "{0}：{1}" + Environment.NewLine +
            "{2}：{3}" + Environment.NewLine +
            "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

            result = serialPort.Read(buffer, offset, count);

            loggerFactory?.GetLogger(Name).Info("已读取数据" + Environment.NewLine +
            "{0}：{1}" + Environment.NewLine +
            "{2}：{3}", nameof(buffer), ConvertHelper.ToString(buffer.Take(result).ToArray()), nameof(result), result);

            return result;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            loggerFactory?.GetLogger(Name).Info("写入数据" + Environment.NewLine +
            "{0}：{1}" + Environment.NewLine +
            "{2}：{3}" + Environment.NewLine +
            "{4}：{5}", nameof(buffer), ConvertHelper.ToString(buffer), nameof(offset), offset, nameof(count), count);

            serialPort.Write(buffer, offset, count);

            loggerFactory?.GetLogger(Name).Info("已写入数据");
        }

        public void Close()
        {
            loggerFactory?.GetLogger(Name).Info("关闭通讯端口");

            serialPort?.Close();
            connectionStateChangedEventManager.StopMonitor( ConnectionStateChanged, this);

            loggerFactory?.GetLogger(Name).Info("已关闭通讯端口");
        }

        public void Dispose()
        {
            this.Close();
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            var handler = DataReceived;

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
        private System.IO.Ports.SerialPort serialPort;
        private SerialPortComPortConfigInfo serialPortConfigInfo;
        private ILoggerFactory loggerFactory;
    }
}
