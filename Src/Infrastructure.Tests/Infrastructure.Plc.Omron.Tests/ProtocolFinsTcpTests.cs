using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;

namespace Infrastructure.Plc.Omron.Tests
{
    [TestFixture()]
    public class ProtocolFinsTcpTests
    {
        [Test]
        public void ErrorCodeTest()
        {
            var bytes = new byte[] {
                0x01,
                0x02,
                0x03,
                0x04
            };

            var value = string.Join(string.Empty, bytes.Select(e => e.ToString("X2")).ToArray());
        }

        [Test, Ignore("验证欧姆龙PLC是否支持多TcpClient连接")]
        public void ConnectTest()
        {
            var comPort1 = new TcpClientComPort(new TcpClientComPortConfigInfo()
            {
                LocalIPAddress = "192.168.88.1",
                LocalPort = 8001,
                RemoteIPAddress = "192.168.88.120",
                RemotePort = 9600,
                ReceiveTimeout = 500,
                SendTimeout = 500
            });
            var protocolFinsTcp1 = new PlcOmronFins(comPort1, 0);
            var comPort2 = new TcpClientComPort(new TcpClientComPortConfigInfo()
            {
                LocalIPAddress = "192.168.88.2",
                LocalPort = 8002,
                RemoteIPAddress = "192.168.88.120",
                RemotePort = 9600,
                ReceiveTimeout = 500,
                SendTimeout = 500
            });
            var protocolFinsTcp2 = new PlcOmronFins(comPort2, 0);

            comPort1.Open();
            comPort2.Open();

            protocolFinsTcp1.Initialize();
            protocolFinsTcp2.Initialize();

            protocolFinsTcp1.Write<short>(new DataAddress()
            {
                Type = DataAddressType.Short,
                Offset = 0,
                Value = "D8816"
            }, new short[] { 2 });

            protocolFinsTcp2.Write<short>(new DataAddress()
            {
                Type = DataAddressType.Short,
                Offset = 0,
                Value = "D8816"
            }, new short[] { 3 });
        }

        [Test()]
        public void BitConverterTest()
        {
            var bytes = new byte[]
            {
                0x00,
                0x00,
                0x00,
                0x10
            };
            var value = BitConverter.ToInt32(bytes.Reverse().ToArray(), 0);
            var intBytes = BitConverter.GetBytes(int.MinValue + 1);
            intBytes = BitConverter.GetBytes((uint)12);
            var shortBytes = BitConverter.GetBytes((short)-1234);
            var floatBytes = BitConverter.GetBytes(-1234F);

            Assert.AreEqual(16, value);
        }

        [Test]
        public void ReadIntTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var values = new int[] { };

            comPort.IsConnected.Returns(true);
            comPort.BytesToRead.Returns(8, 26);
            comPort.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
                ci =>
            {
                Array.Copy(new byte[] { 0x46, 0x49, 0x4E, 0x53, 0x00, 0x00, 0x00, 0x1A }, ci.ArgAt<byte[]>(0), 8);

                return ci.ArgAt<byte[]>(0).Count();
            },
                ci =>
            {
                Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x02, 0x00, 0xC0, 0x00, 0x00, 0x21, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x11, 0x22, 0x33, 0x44 }, ci.ArgAt<byte[]>(0), 26);

                return ci.ArgAt<byte[]>(0).Count();
            }
            );

            values = protocolFinsTcp.Read<int>(new DataAddress()
            {
                Value = "D1234",
                Type = DataAddressType.Int,
                Offset = 0
            }).ToArray();

            Assert.AreEqual(BitConverter.ToInt32(new byte[] { 0x22, 0x11, 0x44, 0x33 }, 0), values[0]);
        }

        [Test]
        public void ReadShortTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var values = new short[] { };

            comPort.IsConnected.Returns(true);
            comPort.BytesToRead.Returns(8, 26);
            comPort.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
                ci =>
                {
                    Array.Copy(new byte[] { 0x46, 0x49, 0x4E, 0x53, 0x00, 0x00, 0x00, 0x1A }, ci.ArgAt<byte[]>(0), 8);

                    return ci.ArgAt<byte[]>(0).Count();
                },
                ci =>
                {
                    Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x02, 0x00, 0xC0, 0x00, 0x00, 0x21, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x11, 0x22, 0x33, 0x44 }, ci.ArgAt<byte[]>(0), 26);

                    return ci.ArgAt<byte[]>(0).Count();
                }
            );

            values = protocolFinsTcp.Read<short>(new DataAddress()
            {
                Value = "D1234",
                Type = DataAddressType.Short,
                Offset = 1
            }).ToArray();

            Assert.AreEqual(BitConverter.ToInt16(new byte[] { 0x22, 0x11 }, 0), values[0]);
            Assert.AreEqual(BitConverter.ToInt16(new byte[] { 0x44, 0x33 }, 0), values[1]);
        }

        [Test]
        public void ReadStringTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var values = new string[] { };

            comPort.IsConnected.Returns(true);
            comPort.BytesToRead.Returns(8, 26);
            comPort.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
                ci =>
                {
                    Array.Copy(new byte[] { 0x46, 0x49, 0x4E, 0x53, 0x00, 0x00, 0x00, 0x1A }, ci.ArgAt<byte[]>(0), 8);

                    return ci.ArgAt<byte[]>(0).Count();
                },
                ci =>
                {
                    Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x02, 0x00, 0xC0, 0x00, 0x00, 0x21, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x31, 0x32, 0x33, 0x34 }, ci.ArgAt<byte[]>(0), 26);

                    return ci.ArgAt<byte[]>(0).Count();
                }
            );

            values = protocolFinsTcp.Read<string>(new DataAddress()
            {
                Value = "D1234",
                Type = DataAddressType.String,
                Offset = 3
            }).ToArray();

            Assert.AreEqual("1234", values[0]);
        }

        [Test]
        public void ReadBoolTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var values = new bool[] { };

            comPort.IsConnected.Returns(true);
            comPort.BytesToRead.Returns(8, 23);
            comPort.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
                ci =>
                {
                    Array.Copy(new byte[] { 0x46, 0x49, 0x4E, 0x53, 0x00, 0x00, 0x00, 0x17 }, ci.ArgAt<byte[]>(0), 8);

                    return ci.ArgAt<byte[]>(0).Count();
                },
                ci =>
                {
                    Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x02, 0x00, 0xC0, 0x00, 0x00, 0x21, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x01 }, ci.ArgAt<byte[]>(0), 23);

                    return ci.ArgAt<byte[]>(0).Count();
                }
            );

            values = protocolFinsTcp.Read<bool>(new DataAddress()
            {
                Value = "D1234",
                Type = DataAddressType.Boolean,
                Offset = 1
            }).ToArray();

            Assert.AreEqual(true, values[0]);
        }

        [Test]
        public void ReadFloatTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var values = new float[] { };

            comPort.IsConnected.Returns(true);
            comPort.BytesToRead.Returns(8, 26);
            comPort.Read(Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>()).Returns(
                ci =>
                {
                    Array.Copy(new byte[] {
                        0x46, 0x49, 0x4E, 0x53,
                        0x00, 0x00, 0x00, 0x1A
                    }, ci.ArgAt<byte[]>(0), 8);

                    return ci.ArgAt<byte[]>(0).Count();
                },
                ci =>
                {
                    Array.Copy(new byte[] { 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x02, 0x00, 0xC0, 0x00, 0x00, 0x21, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x3f, 0x80 }, ci.ArgAt<byte[]>(0), 26);

                    return ci.ArgAt<byte[]>(0).Count();
                }
            );

            values = protocolFinsTcp.Read<float>(new DataAddress()
            {
                Value = "D1234",
                Type = DataAddressType.Float,
                Offset = 0
            }).ToArray();

            Assert.AreEqual(BitConverter.ToSingle(new byte[] { 0x00, 0x00, 0x80, 0x3F }, 0), values[0]);
        }

        [Test]
        public void WriteShortTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var excepted = new byte[] {
                0x46,0x49,0x4E,0x53,
                0x00,0x00,0x00,0x1C,
                0x00,0x00,0x00,0x02,
                0x00,0x00,0x00,0x00,
                0x80,0x00,0x02,
                0x00,0x00,0x00,
                0x00,0x00,0x00,
                0x00,
                0x01, 0x02,
                0x82,
                0x00,0x64,0x00,
                0x00,0x01,
                0x12,0x34
            };

            try
            {
                comPort.BytesToRead.Returns(0);
                protocolFinsTcp.Write<short>(new DataAddress()
                {
                    Value = "D100",
                    Type = DataAddressType.Short,
                    Offset = 0
                }, new short[] { (short)0x1234 });
            }
            catch
            {

            }

            comPort.Received().Write(Arg.Is<byte[]>(e => !e.Except(excepted).Any() && !excepted.Except(e).Any()), 0, excepted.Length);
        }

        [Test]
        public void WriteIntTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var excepted = new byte[] {
                0x46,0x49,0x4E,0x53,
                0x00,0x00,0x00,0x1E,
                0x00,0x00,0x00,0x02,
                0x00,0x00,0x00,0x00,
                0x80,0x00,0x02,
                0x00,0x00,0x00,
                0x00,0x00,0x00,
                0x00,
                0x01, 0x02,
                0x82,
                0x00,0x64,0x00,
                0x00,0x01,
                0x12,0x34,0x56,0x78
            };

            try
            {
                comPort.BytesToRead.Returns(0);
                protocolFinsTcp.Write<int>(new DataAddress()
                {
                    Value = "D100",
                    Type = DataAddressType.Int,
                    Offset = 0
                }, new int[] { 0x12345678 });
            }
            catch
            {

            }

            comPort.Received().Write(Arg.Is<byte[]>(e => !e.Except(excepted).Any() && !excepted.Except(e).Any()), 0, excepted.Length);
        }

        [Test]
        public void WriteStringTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var excepted = new byte[] {
                0x46,0x49,0x4E,0x53,
                0x00,0x00,0x00,0x1E,
                0x00,0x00,0x00,0x02,
                0x00,0x00,0x00,0x00,
                0x80,0x00,0x02,
                0x00,0x00,0x00,
                0x00,0x00,0x00,
                0x00,
                0x01, 0x02,
                0x82,
                0x00,0x64,0x00,
                0x00,0x01,
                0x31,0x32,0x33,0x34
            };

            try
            {
                comPort.BytesToRead.Returns(0);
                protocolFinsTcp.Write(new DataAddress()
                {
                    Value = "D100",
                    Type = DataAddressType.String,
                    Offset = 3
                }, new string[] { "1234" });
            }
            catch
            {

            }

            comPort.Received().Write(Arg.Is<byte[]>(e => !e.Except(excepted).Any() && !excepted.Except(e).Any()), 0, excepted.Length);
        }

        [Test]
        public void WriteBoolTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var excepted = new byte[] {
                0x46,0x49,0x4E,0x53,
                0x00,0x00,0x00,0x1B,
                0x00,0x00,0x00,0x02,
                0x00,0x00,0x00,0x00,
                0x80,0x00,0x02,
                0x00,0x00,0x00,
                0x00,0x00,0x00,
                0x00,
                0x01, 0x02,
                0x02,
                0x00,0x64,0x01,
                0x00,0x01,
                0x00
            };

            try
            {
                protocolFinsTcp.Write(new DataAddress()
                {
                    Value = "D100",
                    Type = DataAddressType.Boolean,
                    Offset = 1
                }, new bool[] { false });
            }
            catch
            {

            }

            comPort.Received().Write(Arg.Is<byte[]>(e => !e.Except(excepted).Any() && !excepted.Except(e).Any()), 0, excepted.Length);
        }

        [Test]
        public void WriteFloatTest()
        {
            var comPort = Substitute.For<IComPort>();
            var protocolFinsTcp = new PlcOmronFins(comPort, 0);
            var excepted = new byte[] {
                0x46,0x49,0x4E,0x53,
                0x00,0x00,0x00,0x1E,
                0x00,0x00,0x00,0x02,
                0x00,0x00,0x00,0x00,
                0x80,0x00,0x02,
                0x00,0x00,0x00,
                0x00,0x00,0x00,
                0x00,
                0x01, 0x02,
                0x82,
                0x00,0x64,0x00,
                0x00,0x01,
                0x3f, 0x80, 0x00,0x00
            };

            try
            {
                comPort.BytesToRead.Returns(0);
                protocolFinsTcp.Write(new DataAddress()
                {
                    Value = "D100",
                    Type = DataAddressType.Float,
                    Offset = 0
                }, new float[] { 1 });
            }
            catch
            {

            }

            comPort.Received().Write(Arg.Is<byte[]>(e => !e.Except(excepted).Any() && !excepted.Except(e).Any()), 0, excepted.Length);
        }
    }
}