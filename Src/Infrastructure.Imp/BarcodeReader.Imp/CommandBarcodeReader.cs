using Infrastructure.BarcodeReader.Interface;
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp;
using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Infrastructure.BarcodeReader.Imp
{
    public class CommandBarcodeReader : BarcodeReaderBase
    {
        public CommandBarcodeReader(string name, IComPort comPort, CommandBarcodeReaderConfigInfo commandBarcodeReaderConfigInfo, string regex = null, ILoggerFactory loggerFactory = null)
            : base(name)
        {
            this.comPort = comPort;
            this.CommandBarcodeReaderConfigInfo = commandBarcodeReaderConfigInfo ?? new CommandBarcodeReaderConfigInfo();
            this.loggerFactory = loggerFactory;
            this.regex = new Regex(regex ?? string.Empty);
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(name);

            this.comPort.ConnectionStateChanged += (s, e) =>
            {
                connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, s, e);
            };
        }

        public override event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public CommandBarcodeReaderConfigInfo CommandBarcodeReaderConfigInfo { get; set; }

        public override string Read()
        {
            var result = BarcodeConst.EmptyBarcode;
            var barcodeReaderReceivedEventArgs = new BarcodeReaderReceivedEventArgs();

            try
            {
                byte[] request = GetBytes(CommandBarcodeReaderConfigInfo.ReadCommand);
                byte[] response = { };

                lock (syncObj)
                {
                    ClearBarcode();

                    comPort.Write(request, 0, request.Length);

                    loggerFactory?.GetLogger<CommandBarcodeReader>().Debug($"已发送读取指令，条码读取器：{Name },指令：{CommandBarcodeReaderConfigInfo.ReadCommand}");

                    result = ReadBarcode();
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = BarcodeConst.EmptyBarcode;
                }
                else
                {
                    if (regex != null && !regex.IsMatch(result)) result = BarcodeConst.ErrorBarcode;
                }
            }
            catch (Exception e)
            {
                result = BarcodeConst.ErrorBarcode;
                loggerFactory?.GetLogger<CommandBarcodeReader>().Error(e.ToString());
            }

            barcodeReaderReceivedEventArgs.Barcode = result;
            OnBarcoderReceived(this, barcodeReaderReceivedEventArgs);
            result = barcodeReaderReceivedEventArgs.Barcode;

            return result;
        }

        private void ClearBarcode()
        {
            byte[] buffer = new byte[1024];
            var data = new List<byte>();
            var count = 0;
            var dt = DateTime.Now.AddSeconds(CommandBarcodeReaderConfigInfo.Timeout);

            do
            {
                if (comPort.BytesToRead != 0)
                {
                    count = comPort.Read(buffer, 0, buffer.Length);
                    data.AddRange(buffer.Take(count));
                }
                else
                {
                    break;
                }
            } while (DateTime.Now <= dt);

            loggerFactory?.GetLogger<CommandBarcodeReader>().Debug($"已清除条码数据，条码读取器：{Name },条码数据：{GetRamString(data.ToArray())}");
        }

        private string ReadBarcode()
        {
            var result = string.Empty;
            var data = new List<byte>();
            var barcodeStartBytes = GetBytes(CommandBarcodeReaderConfigInfo.BarcodeStart);
            var barcodeEndBytes = GetBytes(CommandBarcodeReaderConfigInfo.BarcodeEnd);
            var dt = DateTime.Now.AddSeconds(CommandBarcodeReaderConfigInfo.Timeout);
            var isStartOk = true;
            var isEndOk = true;

            do
            {
                byte[] buffer = new byte[1024];
                int count = 0;

                if (comPort.BytesToRead != 0)
                {
                    count = comPort.Read(buffer, 0, buffer.Length);
                    data.AddRange(buffer.Take(count));
                }
                else
                {
                    ThreadHelper.Sleep(1);
                }

                if ((comPort.BytesToRead == 0) && (data.Count >= barcodeStartBytes.Length + barcodeEndBytes.Length))
                {
                    if (barcodeStartBytes.Any())
                    {
                        for (int index = 0; index < barcodeStartBytes.Length; index++)
                        {
                            if (data[index] != barcodeStartBytes[index])
                            {
                                isStartOk = false;

                                break;
                            }
                        }
                    }

                    if (barcodeEndBytes.Any())
                    {
                        var dataReverse = data.ToArray().Reverse().ToArray();
                        var barcodeEndBytesReverse = barcodeEndBytes.Reverse().ToArray();

                        for (int index = 0; index < barcodeEndBytes.Length; index++)
                        {
                            if (dataReverse[index] != barcodeEndBytesReverse[index])
                            {
                                isEndOk = false;

                                break;
                            }
                        }
                    }

                    if (isStartOk && isEndOk) break;
                }
            } while (DateTime.Now <= dt);

            result = GetString(data.ToArray());

            loggerFactory?.GetLogger<CommandBarcodeReader>().Debug($"已接收条码数据，条码读取器：{Name },条码数据：{GetRamString(data.ToArray())}");

            return result;
        }

        private byte[] GetBytes(string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        private string GetRamString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        private string GetString(byte[] bytes)
        {
            var result = Encoding.ASCII.GetString(bytes);

            if (!string.IsNullOrEmpty(CommandBarcodeReaderConfigInfo.BarcodeStart) && result.StartsWith(CommandBarcodeReaderConfigInfo.BarcodeStart))
            {
                result = result.Substring(CommandBarcodeReaderConfigInfo.BarcodeStart.Length, result.Length - CommandBarcodeReaderConfigInfo.BarcodeStart.Length);
            }

            if (!string.IsNullOrEmpty(CommandBarcodeReaderConfigInfo.BarcodeEnd) && result.EndsWith(CommandBarcodeReaderConfigInfo.BarcodeEnd))
            {
                result = result.Substring(0, result.Length - CommandBarcodeReaderConfigInfo.BarcodeEnd.Length);
            }

            return result;
        }

        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private IComPort comPort;
        private readonly ILoggerFactory loggerFactory;
        private Regex regex;
        private object syncObj = new object();
    }
}
