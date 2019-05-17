using System;

namespace Infrastructure.Log.Interface
{
    /// <summary>
    /// <para>Fatal:Highest level: important stuff down</para>
    /// <para>Error:For example application crashes / exceptions.</para>
    /// <para>Warn:Incorrect behavior but the application can continue</para>
    /// <para>Info:Normal behavior like mail sent, user updated profile etc.</para>
    /// <para>Debug:Executed queries, user authenticated, session expired</para>
    /// <para>Trace:Begin method X, end method X etc</para>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 最高级别：重要的信息
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Fatal(string message, params object[] args);

        /// <summary>
        /// 错误：例如应用程序崩溃、异常
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Error(string message, params object[] args);

        /// <summary>
        /// 警告：错误行为，但应用程序可以继续运行
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Warn(string message, params object[] args);

        /// <summary>
        /// 信息：正常行为，如邮件发送，用户更新配置文件等。
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Info(string message, params object[] args);

        /// <summary>
        /// 调试：执行查询，用户认证，会话过期
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// 跟踪：开始执行方法，方法执行结束等
        /// </summary>
        /// <param name="message">日志消息，可以是完整消息，也可以是格式化字符串</param>
        /// <param name="args">格式化参数</param>
        void Trace(string message, params object[] args);
    }
}