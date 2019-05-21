using Infrastructure.BarcodeReader.Interface;
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Text.RegularExpressions;

namespace Infrastructure.BarcodeReader.Imp
{
    public class PlcBarcodeReader : BarcodeReaderBase
    {
        public PlcBarcodeReader(string name, IPlc plc, DataAddress address, string regex = null)
            : base(name)
        {
            this.plc = plc;
            this.address = address;
            this.regex = new Regex(regex ?? string.Empty);
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(name);

            this.plc.ConnectionStateChanged += (s, e) =>
            {
                connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, s, e);
            };
        }

        public override event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public override string Read()
        {
            var result = BarcodeConst.EmptyBarcode;
            var barcodeReaderReceivedEventArgs = new BarcodeReaderReceivedEventArgs();

            try
            {
                result = plc.ReadSingle<string>(address);

                if (string.IsNullOrEmpty(result)) result = BarcodeConst.EmptyBarcode;

                if (regex != null && !regex.IsMatch(result)) result = BarcodeConst.ErrorBarcode;
            }
            catch
            {
                result = BarcodeConst.ErrorBarcode;
            }

            barcodeReaderReceivedEventArgs.Barcode = result;
            OnBarcoderReceived(this, barcodeReaderReceivedEventArgs);
            result = barcodeReaderReceivedEventArgs.Barcode;

            return result;
        }

        private readonly IPlc plc;
        private readonly DataAddress address;
        private readonly Regex regex;
        private readonly ConnectionStateChangedEventManager connectionStateChangedEventManager;
    }
}
