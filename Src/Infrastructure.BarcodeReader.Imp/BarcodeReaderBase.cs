using Infrastructure.BarcodeReader.Interface;
using Infrastructure.Common.Interface;
using System;

namespace Infrastructure.BarcodeReader.Imp
{
    public abstract class BarcodeReaderBase : IBarcodeReader
    {
        public BarcodeReaderBase(string name)
        {
            this.Name = name;
        }

        public event EventHandler<BarcodeReaderReceivedEventArgs> BarcoderReceived;
        public abstract event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public string Name { get; set; }

        public abstract string Read();

        protected void OnBarcoderReceived(object sender, EventArgs e)
        {
            if (BarcoderReceived != null)
            {
                foreach (var del in BarcoderReceived.GetInvocationList())
                {
                    del.DynamicInvoke(this, e);
                }
            }
        }
    }
}
