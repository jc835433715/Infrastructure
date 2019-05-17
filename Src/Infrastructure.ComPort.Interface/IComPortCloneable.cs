namespace Infrastructure.ComPort.Interface
{
    /// <summary>
    /// 通讯端口克隆接口
    /// </summary>
    public interface IComPortCloneable
    {
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        IComPort Clone();
    }
}
