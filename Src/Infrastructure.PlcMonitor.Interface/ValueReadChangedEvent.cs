using System;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 读取到值改变事件
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    [Serializable]
    public class ValueReadChangedEvent<TValue> : EventBase, IEvent
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public ValueReadChangedEvent()
        {
            this.CurrentValue = default(TValue);
        }

        /// <summary>
        ///值类型
        /// </summary>
        public Type ValueType { get { return typeof(TValue); } }

        /// <summary>
        /// 当前值
        /// </summary>
        public TValue CurrentValue { get; set; }

        /// <summary>
        /// 判断是否值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>值相等，返回true</returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            ValueReadChangedEvent<TValue> other = obj as ValueReadChangedEvent<TValue>;

            result = object.ReferenceEquals(this, other) || (base.Equals(obj) && other != null);

            return result;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
