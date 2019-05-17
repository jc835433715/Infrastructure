using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BarcodeReader.Imp
{
    public class BarcodeReaderConfigInfo
    {
        public BarcodeReaderConfigInfo()
        {
            this.CommandBarcodeReader = new CommandBarcodeReaderConfigInfo();
        }

        public CommandBarcodeReaderConfigInfo CommandBarcodeReader { get; set; }
    }

    public class CommandBarcodeReaderConfigInfo
    {
        public CommandBarcodeReaderConfigInfo()
        {
            this.ReadCommand = string.Empty;
            this.BarcodeStart = string.Empty;
            this.BarcodeEnd = string.Empty;
            this.Timeout = 3;
        }

        public string ReadCommand { get; set; }

        public string BarcodeStart { get; set; }

        public string BarcodeEnd { get; set; }

        public int Timeout { get; set; }
    }
}
