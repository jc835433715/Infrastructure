using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;

namespace Infrastructure.Plc.ManualSimulator
{
    public class WritingEventArgs : EventArgs
    {
        public WritingEventArgs()
        {
            this.Address = DataAddress.Empty;
            this.Values = new List<object>();
        }

        public DataAddress Address { get; set; }

        public ICollection<object> Values { get; set; }
    }
}