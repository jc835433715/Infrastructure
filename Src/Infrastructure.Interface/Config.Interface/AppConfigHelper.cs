using System;
using System.Configuration;
using System.Diagnostics;

namespace Infrastructure.Config.Interface
{
    /// <summary>
    ///App.config读写帮助类
    /// </summary>
    public static class AppConfigHelper
    {
        /// <summary>
        /// 为当前应用程序域属性分配指定值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="data">值</param>
        public static void SetCurrentDomainData(string name, object data)
        {
            AppDomain.CurrentDomain.SetData(name, data);
        }

        /// <summary>
        /// 获取当前应用程序域属性值
        /// </summary>
        /// <param name="name">属性名</param>
        public static object GetCurrentDomainData(string name)
        {
            return AppDomain.CurrentDomain.GetData(name);
        }

        /// <summary>
        /// AppSettings读写类
        /// </summary>
        public static class AppSettings
        {
            /// <summary>
            /// 获取值
            /// </summary>
            /// <param name="key">键</param>
            /// <returns>返回值，读取失败，返回string.Empty</returns>
            public static string Get(string key)
            {
                var result = string.Empty;

                try
                {
                    result = ConfigurationManager.AppSettings[key] ?? string.Empty;
                }
                catch
                {
                }

                return result;
            }

            /// <summary>
            /// 设置值
            /// </summary>
            /// <param name="key">键</param>
            /// <param name="value">值</param>
            public static void Set(string key, object value)
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                appConfig.AppSettings.Settings[key].Value = value.ToString();

                appConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        /// <summary>
        /// ConnectionStrings读写类
        /// </summary>
        public static class ConnectionStrings
        {
            /// <summary>
            /// 获取连接字符串设置
            /// </summary>
            /// <param name="name">连接字符串名称</param>
            /// <returns>返连接字符串设置</returns>
            public static ConnectionStringSettings Get(string name)
            {
                var result = new ConnectionStringSettings();

                try
                {
                    result = ConfigurationManager.ConnectionStrings[name];
                }
                catch
                {
                }

                return result;
            }

            /// <summary>
            /// 设置连接字符串
            /// </summary>
            /// <param name="name">连接字符串名称</param>
            /// <param name="value">连接字符串设置</param>
            public static void Set(string name, ConnectionStringSettings value)
            {
                Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = appConfig.ConnectionStrings.ConnectionStrings[name];
                
                settings.ConnectionString = value.ConnectionString;
                settings.ProviderName = value.ProviderName;

                appConfig.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }
        }
    }
}
