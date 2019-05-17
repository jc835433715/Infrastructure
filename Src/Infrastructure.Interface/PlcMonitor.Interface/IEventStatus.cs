namespace Infrastructure.PlcMonitor.Interface
{
    /// <summary>
    /// 事件状态接口
    /// </summary>
    public interface IEventStatus
    {
        /// <summary>
        /// 是否事件状态处理完成
        /// </summary>
        bool IsHandleCompleted { get; set; }
    }
}
