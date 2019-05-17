using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 读取到值事件
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class ValueReadEvent<TValue> : EventBase, IEvent
    {
        /// <summary>
        /// 默认构造,Period为TimeSpan.FromSeconds(1)
        /// </summary>
        public ValueReadEvent()
        {
            this.Period = TimeSpan.FromSeconds(1);
            this.CurrentValue = default(TValue);
        }

        /// <summary>
        ///值类型
        /// </summary>
        public Type ValueType => typeof(TValue);

        /// <summary>
        /// 条件
        /// </summary>
        public Func<TValue, bool> Predicate { get; set; }

        /// <summary>
        /// 读取事件间隔
        /// </summary>
        public TimeSpan Period { get; set; }

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
            ValueReadEvent<TValue> other = obj as ValueReadEvent<TValue>;

            result = object.ReferenceEquals(this, other) || (base.Equals(obj) && other != null && Period.Equals(other.Period));

            return result;
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns>返回哈希值</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() | Period.GetHashCode();
        }
    }
}
