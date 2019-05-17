using System;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        ///值类型
        /// </summary>
        Type ValueType { get; }
    }
}
