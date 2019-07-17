using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp.Net;
using Infrastructure.ComPort.Interface;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace Infrastructure.Plc.Keyence.Tests
{
    [TestFixture()]
    public class PlcKeyenceUpperLinkTests
    {
        [Test()]
        public void Test()
        {
            TcpClientComPort tcpClientComPort = new TcpClientComPort(new TcpClientComPortConfigInfo()
            {
                RemoteIPAddress ="192.168.88.90",
                RemotePort = 8501,
                SendTimeout = 500,
                ReceiveTimeout = 500
            });
            PlcKeyenceUpperLink protocolHostLink = new PlcKeyenceUpperLink(tcpClientComPort,3);

            try
            {
                tcpClientComPort.Open();

                Thread.Sleep(500);

                var v1 = protocolHostLink.Read<Boolean>(new DataAddress()
                {
                    Value = "MR1100",
                    Type = DataAddressType.Boolean,
                    Offset = 0
                });
                protocolHostLink.Write<Boolean>(new DataAddress()
                {
                    Value = "MR1100",
                    Type = DataAddressType.Boolean,
                    Offset = 0
                }, new Boolean[] { !v1.Single() });

                var v2 = protocolHostLink.Read<short>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Short,
                    Offset = 0
                });
                protocolHostLink.Write<short>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Short,
                    Offset = 0
                }, new short[] { 100 });

                var v3 = protocolHostLink.Read<ushort>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Ushort,
                    Offset = 0
                });
                protocolHostLink.Write<ushort>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Ushort,
                    Offset = 0
                }, new ushort[] { 100 });

                var v4 = protocolHostLink.Read<int>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Int,
                    Offset = 0
                });
                protocolHostLink.Write<int>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Int,
                    Offset = 0
                }, new int[] {100 });

                var v5 = protocolHostLink.Read<float>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Float,
                    Offset = 0
                });
                protocolHostLink.Write<float>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.Float,
                    Offset = 0
                }, new float[] { 0.3f});

                var v6 = protocolHostLink.Read<String>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.String,
                    Offset = 5
                });
                protocolHostLink.Write<String>(new DataAddress()
                {
                    Value = "DM1180",
                    Type = DataAddressType.String,
                    Offset = 5
                }, new string[] { "DM11800" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                tcpClientComPort?.Close();
            }
        }
    }
}