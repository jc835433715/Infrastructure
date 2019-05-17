using Infrastructure.Plc.Interface;
using System.Collections.Generic;

namespace Infrastructure.PlcMonitor.Imp
{

    public class PlcAddressSegment
    {
        public PlcAddressSegment()
        {
            this.StartPlcAddress = PlcAddress.Empty;
            this.AllPlcAddressesByDes = new List<PlcAddress>();
        }

        public PlcAddress StartPlcAddress { get; set; }


        public List<PlcAddress> AllPlcAddressesByDes { get; set; }
    }
}
