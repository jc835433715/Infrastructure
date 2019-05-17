using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// Plc Opc接口
    /// </summary>
    public interface IPlcOpc : IPlc
    {
        /// <summary>
        /// 浏览地址
        /// </summary>
        /// <param name="address">地址</param>
        /// <returns>子地址</returns>
        IEnumerable<PlcAddress> Explore(PlcAddress address);

        /// <summary>
        /// 监视值变化
        /// </summary>
        /// <typeparam name="TValue">类型</typeparam>
        /// <param name="address">地址</param>
        /// <param name="callback">回调,第二个参数为unsubscribe方法</param>
        void Monitor<TValue>(PlcAddress address, Action<TValue, Action> callback);
    }
}
