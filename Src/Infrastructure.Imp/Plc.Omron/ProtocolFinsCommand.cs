using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Omron
{
    static class ProtocolFinsCommand
    {
        public static byte[] GetHandshakeCommand(byte pcNode)
        {
            var command = new List<byte>();

            command.AddRange(new byte[] { 0x46, 0x49, 0x4E, 0x53 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x0C });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, pcNode });

            return command.ToArray();
        }

        public static byte[] GetReadCommand(PlcAddress address, byte pcNode, byte plcNode)
        {
            var command = new List<byte>();

            command.AddRange(new byte[] { 0x46, 0x49, 0x4E, 0x53 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x1A });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x02 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            command.AddRange(new byte[] { 0x80, 0x00, 0x02 });
            command.AddRange(new byte[] { 0x00, plcNode, 0x00 });
            command.AddRange(new byte[] { 0x00, pcNode, 0x00 });
            command.AddRange(new byte[] { 0x00 });
            command.AddRange(new byte[] { 0x01, 0x01 });
            command.AddRange(new byte[] { GetMemoryAreaCode(address) });
            command.AddRange(GetBeginingAddressBytes(address));
            command.AddRange(GetItemCountBytes(address));

            return command.ToArray();
        }

        public static byte[] GetWriteCommand<TValue>(PlcAddress address, IEnumerable<TValue> values, byte pcNode, byte plcNode)
        {
            var command = new List<byte>();
            var datas = values.ToList();
            var valueType = typeof(TValue);

            command.AddRange(new byte[] { 0x46, 0x49, 0x4E, 0x53 });
            command.AddRange(GetLenghtBytes(address));
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x02 });
            command.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            command.AddRange(new byte[] { 0x80, 0x00, 0x02 });
            command.AddRange(new byte[] { 0x00, plcNode, 0x00 });
            command.AddRange(new byte[] { 0x00, pcNode, 0x00 });
            command.AddRange(new byte[] { 0x00 });
            command.AddRange(new byte[] { 0x01, 0x02 });
            command.AddRange(new byte[] { GetMemoryAreaCode(address) });
            command.AddRange(GetBeginingAddressBytes(address));
            command.AddRange(GetItemCountBytes(address));

            switch (address.Type)
            {
                case PlcAddressType.Boolean:
                    {
                        var value = (bool)Convert.ChangeType(datas.Single(), valueType);

                        command.Add((byte)(value ? 1 : 0));
                    }
                    break;
                case PlcAddressType.Short:
                    datas.ForEach(e =>
                    {
                        var value = (short)Convert.ChangeType(e, valueType);
                        var bytes = BitConverter.GetBytes(value);

                        command.AddRange(bytes.Take(2).Reverse().ToArray());
                    });
                    break;
                case PlcAddressType.Ushort:
                    datas.ForEach(e =>
                    {
                        var value = (ushort)Convert.ChangeType(e, valueType);
                        var bytes = BitConverter.GetBytes(value);

                        command.AddRange(bytes.Take(2).Reverse().ToArray());
                    });
                    break;
                case PlcAddressType.Int:
                    datas.ForEach(e =>
                    {
                        var value = (int)Convert.ChangeType(e, valueType);
                        var bytes = BitConverter.GetBytes(value);

                        command.AddRange(bytes.Take(2).Reverse().ToArray());
                        command.AddRange(bytes.Skip(2).Take(2).Reverse().ToArray());
                    });
                    break;

                case PlcAddressType.Float:
                    datas.ForEach(e =>
                    {
                        var value = (float)Convert.ChangeType(e, valueType);
                        var bytes = BitConverter.GetBytes(value);

                        command.AddRange(bytes.Take(2).Reverse().ToArray());
                        command.AddRange(bytes.Skip(2).Take(2).Reverse().ToArray());
                    });
                    break;
                case PlcAddressType.String:
                    {
                        var value = (string)Convert.ChangeType(datas.Single(), valueType);
                        var bytes = Encoding.ASCII.GetBytes(value);
                        var count = ((int)Math.Ceiling((address.Offset + 1) / 2d)) * 2 - bytes.Length;


                        command.AddRange(bytes);

                        while (count-- > 0) command.Add(0x00);
                    }
                    break;

                default: throw new NotImplementedException();
            }

            return command.ToArray();
        }

        private static byte GetMemoryAreaCode(PlcAddress address)
        {
            MemoryAreaType memoryAreaType = MemoryAreaType.None;
            MemoryAreaCode memoryAreaCode;

            if (address.Value.Contains("_"))//判断是否为EM区地址
            {
                memoryAreaType = (MemoryAreaType)Enum.Parse(typeof(MemoryAreaType), address.Value.Substring(0, 2), true);
            }
            else
            {
                memoryAreaType = (MemoryAreaType)Enum.Parse(typeof(MemoryAreaType), address.Value.Substring(0, 1), true);
            }

            memoryAreaCode = MemoryAreaCode.GetMemoryAreaCode(memoryAreaType);

            return address.Type == PlcAddressType.Boolean ? memoryAreaCode.Bit : memoryAreaCode.Word;
        }

        private static byte[] GetBeginingAddressBytes(PlcAddress address)
        {
            var result = new List<byte>();

            if (address.Value.Contains("_"))//判断是否为E区地址
            {
                result = new List<byte>(BitConverter.GetBytes(ushort.Parse(address.Value.Substring(3))).Reverse());
            }
            else
            {
                result = new List<byte>(BitConverter.GetBytes(ushort.Parse(address.Value.Substring(1))).Reverse());
            }

            if (address.Type == PlcAddressType.Boolean)
            {
                result.Add((byte)address.Offset);
            }
            else
            {
                result.Add(0x00);
            }

            return result.ToArray();
        }

        private static byte[] GetItemCountBytes(PlcAddress address)
        {
            int count = 0;

            switch (address.Type)
            {
                case PlcAddressType.Boolean: count = 1; break;
                case PlcAddressType.Short: count = 1 * (address.Offset + 1); break;
                case PlcAddressType.Ushort: count = 1 * (address.Offset + 1); break;
                case PlcAddressType.Int: count = 2 * (address.Offset + 1); break;
                case PlcAddressType.Float: count = 2 * (address.Offset + 1); break;
                case PlcAddressType.String: count = (int)Math.Ceiling((address.Offset + 1) / 2d); break;
                default: throw new NotImplementedException();
            }

            return BitConverter.GetBytes((ushort)count).Reverse().ToArray();
        }

        private static byte[] GetLenghtBytes(PlcAddress address)
        {
            int lenght = 2 * 4 + 3 * 3 + 1 + 2 + 1 + 3 + 2 + 2 * BitConverter.ToUInt16(GetItemCountBytes(address).Reverse().ToArray(), 0) + (address.Type == PlcAddressType.Boolean ? -1 : 0);
            var bytes = BitConverter.GetBytes((ushort)lenght);

            return new byte[] {
                0x00,
                0x00,
                bytes[1],
                bytes[0]
            };
        }
    }

    enum MemoryAreaType
    {
        None,
        D,
        C,
        W,
        H,
        A,
        E0,
        E1,
        E2,
        E3
    }

    class MemoryAreaCode
    {
        public MemoryAreaCode(byte bitCode, byte wordCode)
        {
            Bit = bitCode;
            Word = wordCode;

        }

        public byte Bit { get; private set; }

        public byte Word { get; private set; }

        public static MemoryAreaCode GetMemoryAreaCode(MemoryAreaType memoryAreaType)
        {
            switch (memoryAreaType)
            {
                case MemoryAreaType.D: return DM;
                case MemoryAreaType.C: return CIO;
                case MemoryAreaType.W: return WR;
                case MemoryAreaType.H: return HR;
                case MemoryAreaType.A: return AR;
                case MemoryAreaType.E0: return EM0;
                case MemoryAreaType.E1: return EM1;
                case MemoryAreaType.E2: return EM2;
                case MemoryAreaType.E3: return EM3;
                default: return DM;
            }
        }

        public static readonly MemoryAreaCode DM = new MemoryAreaCode(0x02, 0x82);
        public static readonly MemoryAreaCode CIO = new MemoryAreaCode(0x30, 0xB0);
        public static readonly MemoryAreaCode WR = new MemoryAreaCode(0x31, 0xB1);
        public static readonly MemoryAreaCode HR = new MemoryAreaCode(0x32, 0xB2);
        public static readonly MemoryAreaCode AR = new MemoryAreaCode(0x33, 0xB3);
        public static readonly MemoryAreaCode EM0 = new MemoryAreaCode(0x20, 0xA0);
        public static readonly MemoryAreaCode EM1 = new MemoryAreaCode(0x21, 0xA1);
        public static readonly MemoryAreaCode EM2 = new MemoryAreaCode(0x22, 0xA2);
        public static readonly MemoryAreaCode EM3 = new MemoryAreaCode(0x23, 0xA3);
    }

}
