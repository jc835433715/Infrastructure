using Infrastructure.Plc.Interface;
using System;

namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// Plc监视器
    /// </summary>
    public interface IPlcMonitor
    {
        /// <summary>
        /// Plc
        /// </summary>
        IPlc Plc { get; }

        /// <summary>
        /// 启动监控
        /// </summary>
        /// <returns>成功,返回true
        /// </returns>
        bool Start();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="event">事件</param>
        /// <param name="callback">回调</param>
        void Register<TEvent>(TEvent @event, Action<TEvent> callback)
            where TEvent : EventBase, IEvent, new();

        /// <summary>
        /// 注销事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="event">事件</param>
        void Unregister<TEvent>(TEvent @event)
            where TEvent : EventBase, IEvent, new();

        /// <summary>
        /// 清除所有注册事件
        /// </summary>
        void Clear();

        /// <summary>
        /// 停止监控
        /// </summary>
        void Stop();
    }
}