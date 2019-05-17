using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Keyence
{
    static class ProtocolUpperLinkCommand
    {
        public static byte[] GetReadCommand(PlcAddress address)
        {
            var command = new StringBuilder();

            command.Append($"RDS {address.Value }");

            switch (address.Type)
            {
                case PlcAddressType.Boolean:
                    command.Append($" { 1}");
                    break;
                case PlcAddressType.Short:
                    command.Append($".S {address.Offset + 1}");
                    break;
                case PlcAddressType.Ushort:
                    command.Append($".U {address.Offset + 1}");
                    break;
                case PlcAddressType.Int:
                    command.Append($".L {address.Offset + 1}");
                    break;
                case PlcAddressType.Float:
                    command.Append($".H {(address.Offset + 1) * 2}");
                    break;
                case PlcAddressType.String:
                    command.Append($".H {(int)Math.Ceiling((address.Offset + 1) / 2d)}");
                    break;
                default: throw new NotImplementedException();
            }

            command.Append("\r");

            return Encoding.ASCII.GetBytes(command.ToString());
        }

        public static byte[] GetWriteCommand<TValue>(PlcAddress address, IEnumerable<TValue> values)
        {
            var command = new StringBuilder();
            var valueType = typeof(TValue);
            var datas = values.ToList();

            command.Append($"WRS {address.Value }");

            switch (address.Type)
            {
                case PlcAddressType.Boolean:
                    {
                        var value = (bool)Convert.ChangeType(datas.Single(), valueType);

                        command.Append($" {1}");
                        command.Append($" {(value ? 1 : 0)}");
                    }
                    break;
                case PlcAddressType.Short:
                    {
                        command.Append($".S {address.Offset + 1}");

                        datas.ForEach(e =>
                        {
                            command.Append($" {e}");
                        });
                    }
                    break;
                case PlcAddressType.Ushort:
                    {
                        command.Append($".U {address.Offset + 1}");

                        datas.ForEach(e =>
                        {
                            command.Append($" {e}");
                        });
                    }
                    break;
                case PlcAddressType.Int:
                    {
                        command.Append($".L {address.Offset + 1}");

                        datas.ForEach(e =>
                        {
                            command.Append($" {e}");
                        });
                    }
                    break;
                case PlcAddressType.Float:
                    {
                        command.Append($".H {(address.Offset + 1) * 2}");

                        datas.ForEach(e =>
                        {
                            var value = (float)Convert.ChangeType(e, valueType);
                            var bytes = BitConverter.GetBytes(value);

                            command.Append($" {string.Join(string.Empty, bytes.Take(2).Reverse().Select(x => x.ToString("X2")).ToArray())}");
                            command.Append($" {string.Join(string.Empty, bytes.Skip(2).Take(2).Reverse().Select(x => x.ToString("X2")).ToArray())}");
                        });
                    }
                    break;
                case PlcAddressType.String:
                    {
                        var wordCount = (int)Math.Ceiling((address.Offset + 1) / 2d);
                        var value = (string)Convert.ChangeType(datas.Single(), valueType);
                        var bytes = new List<byte>(Encoding.ASCII.GetBytes(value));
                        var count = wordCount * 2 - bytes.Count;

                        command.Append($".H {wordCount}");

                        while (count-- > 0) bytes.Add(0x00);
                        for (int i = 0; i < wordCount; i++)
                        {
                            command.Append($"  {string.Join(string.Empty, bytes.Skip(i * 2).Take(2).Select(x => x.ToString("X2")).ToArray())}");
                        }
                    }
                    break;
                default: throw new NotImplementedException();
            }

            command.Append("\r");

            return Encoding.ASCII.GetBytes(command.ToString());
        }
    }
}
