using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Infrastructure.Plc.ManualSimulator
{
    public class ReadindgEventArgs : EventArgs
    {
        public ReadindgEventArgs()
        {
            this.Address = PlcAddress.Empty;
            this.ManualResetEvent = new ManualResetEvent(false );
            this.Result = new List<object>();
        }

        public PlcAddress Address { get; set; }

        public ManualResetEvent ManualResetEvent { get; set; }

        public List<object> Result;
    }
}