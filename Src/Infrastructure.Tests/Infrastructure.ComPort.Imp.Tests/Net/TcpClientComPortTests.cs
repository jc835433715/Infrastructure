using Infrastructure.ComPort.Interface;
using Infrastructure.Log.Interface;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Infrastructure.ComPort.Imp.Net.Tests
{
    [TestFixture()]
    public class TcpClientComPortTests
    {
        private TcpListener tcpListener;
        private IComPort tcpClientComPort;
        private TcpClient tcpClient;
        private bool connectionState;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.connectionState = false;
            this.tcpListener = new TcpListener(IPAddress.Any, 8080);
            this.tcpClientComPort = new AutoReconnectComPort(
                new TcpClientComPort(
                    new TcpClientComPortConfigInfo()
                    {
                        RemoteIPAddress = "127.0.0.1",
                        RemotePort = 8080,
                        ReceiveTimeout = 500,
                        SendTimeout = 500
                    },
            Substitute.For<ILoggerFactory>()));
            this.tcpClientComPort.ConnectionStateChanged += (sender, e) =>
            {
                connectionState = e.IsConnected;
            };

            tcpListener.Start();
            AcceptTcpClientAsync(tcpListener);
        }

        private async void AcceptTcpClientAsync(TcpListener tcpListener)
        {
            tcpClient = await tcpListener.AcceptTcpClientAsync();
        }

        [Test(), Order(0)]
        public void OpenTest()
        {
            tcpClientComPort.Open(false);

            Thread.Sleep(500);

            Assert.IsTrue(tcpClientComPort.IsConnected);
            Assert.IsTrue(connectionState);
        }

        [Test(), Order(1)]
        public void WriteTest()
        {
            byte[] buffer = Encoding.ASCII.GetBytes("Hello");
            byte[] receivedBytes = new byte[1024];
            int receivedBytesCount = 0;

            tcpClientComPort.Write(buffer, 0, buffer.Length);

            Assert.IsTrue(tcpClient.Client.Available == buffer.Length);

            receivedBytesCount = tcpClient.GetStream().Read(receivedBytes, 0, receivedBytes.Length);

            Assert.IsTrue(receivedBytesCount == buffer.Length);
            Assert.AreEqual("Hello", Encoding.ASCII.GetString(receivedBytes, 0, receivedBytesCount));
        }

        [Test(), Order(2)]
        public void ReadTest()
        {
            byte[] buffer = Encoding.ASCII.GetBytes("Hello");
            byte[] receivedBytes = new byte[1024];
            int receivedBytesCount = 0;

            tcpClient.GetStream().Write(buffer, 0, buffer.Length);

            Assert.IsTrue(tcpClientComPort.BytesToRead == buffer.Length);

            receivedBytesCount = tcpClientComPort.Read(receivedBytes, 0, receivedBytes.Length);

            Assert.IsTrue(receivedBytesCount == buffer.Length);
            Assert.AreEqual("Hello", Encoding.ASCII.GetString(receivedBytes, 0, receivedBytesCount));
        }

        [Test(), Order(3)]
        public void AutoReconnetTest()
        {
            tcpClient.Close();
            tcpClient = null;

            AcceptTcpClientAsync(tcpListener);

            try
            {
                tcpClientComPort.Write(new byte[] {1 }, 0, 1);
                tcpClientComPort.Write(new byte[] { }, 0, 0);
            }
            catch
            {

            }

            Thread.Sleep(500);

            Assert.IsNotNull(tcpClient);
            Assert.IsTrue(connectionState);
        }

        [Test(), Order(4)]
        public void CloseTest()
        {
            tcpClientComPort.Close();

            Assert.IsFalse(connectionState);
        }
    }
}