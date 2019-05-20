using Infrastructure.Log.Interface;
using Infrastructure.Log.NLog;
using Infrastructure.Utils;
using System;
using System.IO;
using System.Linq;

namespace Infrastructure.Log.Factory
{
    /// <summary>
    /// 日志管理者
    /// </summary>
    public static class LogManager
    {
        static LogManager()
        {
            loggerFactory = new NLoggerFactory();
        }

        /// <summary>
        /// 当前日志记录器工厂，默认为NLoggerFactory
        /// </summary>
        public static ILoggerFactory CurrentLoggerFactory => loggerFactory;

        /// <summary>
        /// 获取日志记录器
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>日志记录器</returns>
        public static ILogger GetLogger<T>()
        {
            return loggerFactory.GetLogger<T>();
        }

        /// <summary>
        /// 获取日志记录器
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>日志记录器</returns>
        public static ILogger GetLogger(string name)
        {
            return loggerFactory.GetLogger(name);
        }
        
        /// <summary>
        /// 清理日志
        /// </summary>
        /// <param name="logDirectory">日志目录</param>
        /// <param name="maxDay">日志保留最大天数</param>
        public static void Clear(string logDirectory, int maxDay)
        {
            try
            {
                var queryDirectories = from d in FileSystemHelper.GetDirectories(logDirectory)
                                       where DateTime.Now - DateTime.Parse(d.Split('\\').Last()) >= TimeSpan.FromDays(maxDay)
                                       select d;

                queryDirectories.ToList().ForEach(e => Directory.Delete(e, true));
            }
            catch (Exception e)
            {
                GetLogger("Infrastructure.Utils.LogManager").Error(e.ToString());
            }
        }

        private static ILoggerFactory loggerFactory;
    }
}
