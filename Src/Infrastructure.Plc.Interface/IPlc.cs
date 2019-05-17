using Infrastructure.Common.Interface;
using System.Collections.Generic;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// Plc接口
    /// </summary>
    public interface IPlc: INotifyConnectionStateChanged
    {
        /// <summary>
        /// 初始化Plc,初始化失败，抛出ApplicationException异常
        /// </summary>
        void Initialize();

        /// <summary>
        /// 连续读取
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <returns>返回值</returns>
        IEnumerable<TValue> Read<TValue>(PlcAddress address);

        /// <summary>
        /// 连续写入
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="values">T类型的数组</param>
        void Write<TValue>(PlcAddress address, IEnumerable<TValue> values);

        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
    }
}