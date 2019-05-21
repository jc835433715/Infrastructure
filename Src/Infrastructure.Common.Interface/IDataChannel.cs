using System.Collections.Generic;

namespace Infrastructure.Common.Interface
{
    /// <summary>
    /// 数据信道接口
    /// </summary>
    public interface IDataChannel : INotifyConnectionStateChanged
    {
        /// <summary>
        /// 初始化,初始化失败，抛出ApplicationException异常
        /// </summary>
        void Initialize();

        /// <summary>
        /// 连续读取
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>返回值</returns>
        IEnumerable<TValue> Read<TValue>(DataAddress address);

        /// <summary>
        /// 连续写入
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="values">T类型的数组</param>
        void Write<TValue>(DataAddress address, IEnumerable<TValue> values);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}
