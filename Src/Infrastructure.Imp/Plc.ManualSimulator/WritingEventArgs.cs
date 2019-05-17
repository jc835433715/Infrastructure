using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;

namespace Infrastructure.Plc.ManualSimulator
{
    public class WritingEventArgs : EventArgs
    {
        public WritingEventArgs()
        {
            this.Address = PlcAddress.Empty;
            this.Values = new List<object>();
        }

        public PlcAddress Address { get; set; }

        public ICollection<object> Values { get; set; }
    }
}