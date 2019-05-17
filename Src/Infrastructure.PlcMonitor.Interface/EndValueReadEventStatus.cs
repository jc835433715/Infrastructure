using System;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 读取到终点值状态事件
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class EndValueReadEventStatus<TValue> : EventBase, IEvent, IEventStatus
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public EndValueReadEventStatus()
        {
            this.EndValue = default(TValue);
            this.IsHandleCompleted = false;
        }

        /// <summary>
        ///值类型
        /// </summary>
        public Type ValueType { get { return typeof(TValue); } }

        /// <summary>
        /// 终点值
        /// </summary>
        public TValue EndValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsHandleCompleted { get; set; }

        /// <summary>
        /// 判断是否值相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>值相等，返回true</returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            EndValueReadEventStatus<TValue> other = obj as EndValueReadEventStatus<TValue>;

            result = object.ReferenceEquals(this, other) || (base.Equals(obj) && other != null && EndValue.Equals(other.EndValue));

            return result;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() | EndValue.GetHashCode();
        }
    }
}
