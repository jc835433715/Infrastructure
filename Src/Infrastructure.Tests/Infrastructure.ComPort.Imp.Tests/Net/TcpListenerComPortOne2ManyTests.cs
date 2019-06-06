using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Infrastructure.ComPort.Imp.Net.Tests
{
    [TestFixture()]
    public class TcpListenerComPortOne2ManyTests
    {
        private TcpListenerComPort tcpListenerComPort;
        private TcpClient tcpClient;
        private bool connectionState;
        private TcpClientInfo tcpClientInfo;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.connectionState = false;
            this.tcpListenerComPort = new TcpListenerComPort(new TcpListenerComPortConfigInfo()
            {
                LocalIPAddress = "127.0.0.1",
                LocalPort = 8080,
                ReceiveTimeout = 500,
                SendTimeout = 500
            },
            TcpListenerComPortType.One2Many,
             Substitute.For<ILoggerFactory>());
            this.tcpListenerComPort.ConnectionStateChanged += (sender, e) => connectionState = e.IsConnected; ;
            this.tcpListenerComPort.TcpClientAccepted += (sender, e) => tcpClientInfo = e.TcpClientInfo;
            this.tcpClient = new TcpClient();
        }

        [Test(), Order(0)]
        public void OpenTest()
        {
            tcpListenerComPort.Open();

            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);

            Thread.Sleep(500);

            Assert.IsTrue(connectionState);
            Assert.IsNotNull(tcpClientInfo);
        }

        [Test(), Order(1)]
        public void InvalidOperationExceptionTest()
        {
            Assert.DoesNotThrow(() =>
            {
                try
                {
                    tcpListenerComPort.Write(new byte[] { }, 0, 0);
                }
                catch (InvalidOperationException) { }

                try
                {
                    tcpListenerComPort.Read(new byte[] { }, 0, 0);
                }
                catch (InvalidOperationException) { }

                try
                {
                    var bytesToRead = tcpListenerComPort.BytesToRead;
                }
                catch (InvalidOperationException) { }
            });

        }
        [Test(), Order(2)]
        public void TcpClientInfosTest()
        {
            byte[] buffer = Encoding.ASCII.GetBytes("Hello");
            byte[] receivedBytes = new byte[1024];
            int receivedBytesCount = 0;

            tcpListenerComPort.TcpClientInfos.Single().ComPort.Write(buffer, 0, buffer.Length);

            Assert.IsTrue(tcpClient.Available == buffer.Length);

            receivedBytesCount = tcpClient.GetStream().Read(receivedBytes, 0, receivedBytes.Length);

            Assert.IsTrue(receivedBytesCount == buffer.Length);
            Assert.AreEqual("Hello", Encoding.ASCII.GetString(receivedBytes, 0, receivedBytesCount));
        }

        [Test(), Order(3)]
        public void AutoReconnetTest()
        {
            tcpClient.Close();
            tcpClientInfo = null;

            tcpClient = new TcpClient();

            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);

            Thread.Sleep(500);

            Assert.IsNotNull(tcpClientInfo);
        }

        [Test(), Order(4)]
        public void CloseTest()
        {
            tcpListenerComPort.Close();

            Assert.IsFalse(connectionState);
        }
    }
}