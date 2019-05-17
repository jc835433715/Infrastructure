using System;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 读取到从起点值到终点值事件
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    [Serializable]
    public class FromStartValueToEndValueReadEvent<TValue> : EventBase, IEvent
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public FromStartValueToEndValueReadEvent()
        {
            this.StartValue = default(TValue);
            this.EndValue = default(TValue);
        }

        /// <summary>
        ///值类型
        /// </summary>
        public Type ValueType { get { return typeof(TValue); } }

        /// <summary>
        /// 起点值
        /// </summary>
        public TValue StartValue { get; set; }

        /// <summary>
        /// 终点值
        /// </summary>
        public TValue EndValue { get; set; }

        /// <summary>
        /// 判断是否值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>值相等，返回true</returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            FromStartValueToEndValueReadEvent<TValue> other = obj as FromStartValueToEndValueReadEvent<TValue>;

            result = object.ReferenceEquals(this, other) || (base.Equals(obj) && other != null && StartValue.Equals(other.StartValue) && EndValue.Equals(other.EndValue));

            return result;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() | StartValue.GetHashCode() | EndValue.GetHashCode();
        }
    }
}
