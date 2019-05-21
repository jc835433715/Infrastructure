using Infrastructure.Common.Interface;
using System;

namespace Infrastructure.Modbus.NModbus4
{
    class ModbusAddress
    {
        public byte SlaveNumber { get; set; }

        public IOType IOType { get; set; }

        public ushort StartAddress { get; set; }

        public ushort Length { get; set; }

        public static ModbusAddress Parse(DataAddress address)
        {
            var values = address.Value.Split(':', '：');

            return new ModbusAddress()
            {
                SlaveNumber = byte.Parse(values[0]),
                IOType = (IOType)Enum.Parse(typeof(IOType), values[1], true),
                StartAddress = ushort.Parse(values[2]),
                Length = GetLength(address)
            };
        }

        private static ushort GetLength(DataAddress address)
        {
            var result = 0;

            switch (address.Type)
            {
                case DataAddressType.Boolean: result = (address.Offset + 1); break;
                case DataAddressType.Short: result = (address.Offset + 1); break;
                case DataAddressType.Ushort: result = (address.Offset + 1); break;
                case DataAddressType.Int: result = 2 * (address.Offset + 1); break;
                case DataAddressType.Float: result = 2 * (address.Offset + 1); break;
                case DataAddressType.String: result = (int)Math.Ceiling((address.Offset + 1) / 2d); break;
                default: throw new NotImplementedException();
            }

            return (ushort)result;
        }
    }

    enum IOType
    {
        DI,
        DO,
        AI,
        AO
    }
}
