using Infrastructure.Common.Interface;
using Infrastructure.Log.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.PlcMonitor.Interface;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.PlcMonitor.Imp.Tests
{
    [TestFixture()]
    public class PlcMonitorTests
    {
        private PlcMonitorImp plcMonitorImp;
        private DataAddress address;
        private bool isEndValueReadEventCallback;
        private bool isEndValueReadEventStatusCallback;
        private bool isFromStartValueToEndValueReadEventCallback;
        private bool isNotStartValueReadEventCallback;
        private bool isValueReadChangedEventCallback;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IPlc plc = Substitute.For<IPlc>();
            ILoggerFactory loggerFactory = Substitute.For<ILoggerFactory>();

            this.isEndValueReadEventCallback = false;
            this.isEndValueReadEventStatusCallback = false;
            this.isFromStartValueToEndValueReadEventCallback = false;
            this.isNotStartValueReadEventCallback = false;
            this.isValueReadChangedEventCallback = false;
            this.plcMonitorImp = new PlcMonitorImp(plc, loggerFactory);
            this.address = new DataAddress
            {
                Name = "Adddress",
                Type = DataAddressType.Int,
                Value = "E0_300",
                Offset = 0
            };

            plc.Initialize();
            plc.Read<int>(address).Returns(
             new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 },
             new int[] { 1 }, new int[] { 1 }, new int[] { 1 }, new int[] { 1 }, new int[] { 1 },
             new int[] { 3 }, new int[] { 3 }, new int[] { 3 }, new int[] { 3 }, new int[] { 3 });
        }

        [Test]
        public void ListTest()
        {
            List<int> list = new List<int>();

            list.Remove(3);
        }

        [Test]
        public void TaskTest()
        {
            Exception ex = null;

            try
            {
                Task.Factory.StartNew(() =>
                            {
                                throw new Exception();
                            }).Wait();
            }
            catch (Exception e)
            {
                ex = e;
            }

            try
            {
                Task.Factory.StartNew(() =>
                                 {
                                     throw new Exception();
                                 }).ContinueWith(t =>
                                 {
                                     if (t.IsFaulted)
                                     {
                                         ex = t.Exception.GetBaseException();
                                     }
                                 });
            }
            catch
            {
                throw;
            }

            Assert.IsNotNull(ex);
        }

        [Test(), Order(0)]
        public void RegisterTest()
        {
            plcMonitorImp.Register(new EndValueReadEvent<int>()
            {
                PlcAddress = address,
                EndValue = 1
            }, e =>
            {
                isEndValueReadEventCallback = true;

            });
            plcMonitorImp.Register(new EndValueReadEventStatus<int>()
            {
                PlcAddress = address,
                EndValue = 1
            }, e =>
            {
                isEndValueReadEventStatusCallback = true;
                e.IsHandleCompleted = true;
            });
            plcMonitorImp.Register(new FromStartValueToEndValueReadEvent<int>()
            {
                PlcAddress = address,
                StartValue = 0,
                EndValue = 1
            }, e =>
            {
                isFromStartValueToEndValueReadEventCallback = true;
            });
            plcMonitorImp.Register(new NotStartValueReadEvent<int>()
            {
                PlcAddress = address,
                StartValue = 1
            }, e =>
            {
                isNotStartValueReadEventCallback = true;

                Assert.AreEqual(3, e.CurrentValue);
            });
            plcMonitorImp.Register(new ValueReadChangedEvent<int>()
            {
                PlcAddress = address,
            }, e =>
            {
                isValueReadChangedEventCallback = true;
            });
        }

        [Test(), Order(1)]
        public void StartTest()
        {
            var result = plcMonitorImp.Start();

            Thread.Sleep(1000);

            Assert.IsTrue(result);
            Assert.IsTrue(isEndValueReadEventCallback);
            Assert.IsTrue(isEndValueReadEventStatusCallback);
            Assert.IsTrue(isFromStartValueToEndValueReadEventCallback);
            Assert.IsTrue(isNotStartValueReadEventCallback);
            Assert.IsTrue(isValueReadChangedEventCallback);
        }
        
        [Test(), Order(2)]
        public void UnregisterTest()
        {
            plcMonitorImp.Unregister(new ValueReadChangedEvent<int>()
            {
                PlcAddress = address,
            });
        }

        [Test(), Order(4)]
        public void StopTest()
        {
            plcMonitorImp.Stop();
        }
    }
}