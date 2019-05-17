using System;

namespace Infrastructure.Log.Interface
{
    /// <summary>
    /// 日志消息写入事件参数
    /// </summary>
    public class MessageWritedEventArgs : EventArgs
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public MessageWritedEventArgs()
        {
            this.LoggerName = string.Empty;
            this.Message = string.Empty;
            this.LogLevel = LogLevel.None;
        }

        /// <summary>
        /// 日志记录器名
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        ///Highest level: important stuff down
        /// </summary>
        Fatal,
        /// <summary>
        ///For example application crashes
        /// </summary>
        Error,
        /// <summary>
        ///Incorrect behavior but the application can continue 
        /// </summary>
        Warn,
        /// <summary>
        ///Normal behavior like mail sent, user updated profile etc
        /// </summary>
        Info,
        /// <summary>
        ///Executed queries, user authenticated, session expired
        /// </summary>
        Debug,
        /// <summary>
        ///Begin method X, end method X etc
        /// </summary>
        Trace
    }
}