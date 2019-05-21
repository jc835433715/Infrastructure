using Infrastructure.Opc.HOpc;
using Infrastructure.Opc.Interface;
using Infrastructure.Opc.OpcUaHelper;
using Infrastructure.Plc.Interface;

namespace Infrastructure.Opc.Factory
{
    /// <summary>
    /// Opc工厂
    /// </summary>
    public static class OpcFactory
    {
        /// <summary>
        /// 创建Opc客户端
        /// </summary>
        /// <param name="uriString">uri</param>
        /// <param name="opcClientOptions">Opc客户端配置</param>
        /// <returns>PlcOpc对象</returns>
        public static IOpcClient CreateOpcClientByOpcUaHelper(string uriString, OpcClientOptions opcClientOptions = null)
        {
            return new OpcClientByOpcUaHelper(uriString, opcClientOptions);
        }

        /// <summary>
        /// 创建Opc客户端
        /// </summary>
        /// <param name="uriString">uri</param>
        /// <param name="opcClientOptions">Opc客户端配置</param>
        /// <returns>PlcOpc对象</returns>
        public static IOpcClient CreateOpcClientByHOpc(string uriString, OpcClientOptions opcClientOptions = null)
        {
            return new OpcClientByHOpc(uriString, opcClientOptions);
        }
    }
}
