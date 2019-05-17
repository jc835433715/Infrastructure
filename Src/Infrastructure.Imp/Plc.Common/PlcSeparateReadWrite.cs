using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Imp;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Plc.Common
{
    /// <summary>
    /// 实现读写分离的Plc
    /// </summary>
    public class PlcSeparateReadWrite : IPlc
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="comPortCloneable">通讯端口克隆接口</param>
        /// <param name="createPlcFunc">创建Plc工厂方法</param>
        /// <param name="readTimeoutSeconds">读取超时</param>
        /// <param name="plcReaderCount">用于读取的Plc对象个数</param>
        ///  <param name="plcWriterCount">用于写入的Plc对象个数</param>
        public PlcSeparateReadWrite(
            Func<IComPort, int, IPlc> createPlcFunc,
            IComPortCloneable comPortCloneable,
            int readTimeoutSeconds = 3, int plcReaderCount = 1, int plcWriterCount = 1)
        {
            this.comPortCloneable = comPortCloneable;
            this.createPlcFunc = createPlcFunc;
            this.readTimeoutSeconds = readTimeoutSeconds;
            this.plcReaderCount = plcReaderCount;
            this.plcWriterCount = plcWriterCount;
            this.readCount = 0;
            this.writeCount = 0;
            this.plcReaders = new List<IPlc>();
            this.plcWriters = new List<IPlc>();

            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(string.Empty);
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        /// <summary>
        /// 初始化Plc,初始化失败，抛出ApplicationException异常
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < plcReaderCount; i++)
            {
                plcReaders.Add(CreatePlc());
            }

            for (int i = 0; i < plcWriterCount; i++)
            {
                plcWriters.Add(CreatePlc());
            }

            plcReaders.ForEach(e => e.Initialize());
            plcWriters.ForEach(e => e.Initialize());

            plcReaders.Union(plcWriters).ToList().ForEach(plc =>
            {
                plc.ConnectionStateChanged += (s, e) =>
                {
                    connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged,s, e);
                };
            });
        }

        /// <summary>
        /// 连续读取
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>返回值</returns>
        public IEnumerable<TValue> Read<TValue>(PlcAddress address)
        {
            return plcReaders[(readCount++) % plcReaderCount].Read<TValue>(address);
        }

        /// <summary>
        /// 连续写入
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="values">T类型的数组</param>
        public void Write<TValue>(PlcAddress address, IEnumerable<TValue> values)
        {
            plcWriters[(writeCount++) % plcWriterCount].Write(address, values);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            plcReaders.Union(plcWriters).ToList().ForEach(e => e.Close());
        }

        private IPlc CreatePlc()
        {
            var comPort = comPortCloneable.Clone();

            comPort.Open(false);

            return createPlcFunc(comPort, readTimeoutSeconds);
        }

        private volatile ushort writeCount;
        private volatile ushort readCount;
        private List<IPlc> plcReaders;
        private List<IPlc> plcWriters;
        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
        private readonly IComPortCloneable comPortCloneable;
        private readonly Func<IComPort, int, IPlc> createPlcFunc;
        private readonly int readTimeoutSeconds;
        private readonly int plcReaderCount;
        private readonly int plcWriterCount;
    }
}
