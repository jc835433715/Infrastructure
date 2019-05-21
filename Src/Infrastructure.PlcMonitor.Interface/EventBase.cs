using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 事件基类
    /// </summary>
    [Serializable]
    public abstract class EventBase : ICloneable
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public EventBase()
        {
            this.PlcAddress = DataAddress.Empty;
        }

        /// <summary>
        /// 地址
        /// </summary>
        public DataAddress PlcAddress { get; set; }

        /// <summary>
        /// 判断是否值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>值相等，返回true</returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            EventBase other = obj as EventBase;

            result = object.ReferenceEquals(this, other) || (other != null && PlcAddress.Equals(other.PlcAddress));

            return result;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return PlcAddress.GetHashCode();
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns>返回拷贝对象</returns>
        public object Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);

                return Convert.ChangeType(bf.Deserialize(ms), this.GetType());
            }
        }
    }
}
