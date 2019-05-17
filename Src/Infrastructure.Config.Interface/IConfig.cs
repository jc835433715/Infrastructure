namespace Infrastructure.Config.Interface
{
    /// <summary>
    /// 配置文件接口
    /// </summary>
    /// <typeparam name="TConfigInfo">配置文件映射类型</typeparam>
    public interface IConfig<TConfigInfo> where TConfigInfo : class, new()
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <returns>配置文件映射对象</returns>
        TConfigInfo Read();

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="configInfo">配置文件映射类型</param>
        void Write(TConfigInfo configInfo);
    }
}
