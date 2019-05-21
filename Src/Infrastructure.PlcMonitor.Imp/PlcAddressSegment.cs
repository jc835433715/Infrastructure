using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System.Collections.Generic;

namespace Infrastructure.PlcMonitor.Imp
{

    public class PlcAddressSegment
    {
        public PlcAddressSegment()
        {
            this.StartPlcAddress = DataAddress.Empty;
            this.AllPlcAddressesByDes = new List<DataAddress>();
        }

        public DataAddress StartPlcAddress { get; set; }


        public List<DataAddress> AllPlcAddressesByDes { get; set; }
    }
}
