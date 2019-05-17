namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// Opc客户端配置
    /// </summary>
    public class PlcOpcClientOptions
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// PlcOpc实现类型
        /// </summary>
        public PlcOpcImpType PlcOpcImpType { get; set; }
    }
}
