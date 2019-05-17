using System;
using NLog;

namespace Infrastructure.Log.NLog
{
    public class NLogger : Infrastructure.Log.Interface.ILogger
    {
        public NLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public string NameForUT => logger.Name;

        public void Fatal(string message, params object[] args)
        {
            logger.Fatal(message, args);

            OnMessageWrited(logger.IsFatalEnabled, Interface.LogLevel.Fatal, message, args);
        }

        public void Error(string message, params object[] args)
        {
            logger.Error(message, args);

            OnMessageWrited(logger.IsErrorEnabled, Interface.LogLevel.Error, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            logger.Warn(message, args);

            OnMessageWrited(logger.IsWarnEnabled, Interface.LogLevel.Warn, message, args);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message, args);

            OnMessageWrited(logger.IsInfoEnabled, Interface.LogLevel.Info, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            logger.Debug(message, args);

            OnMessageWrited(logger.IsDebugEnabled, Interface.LogLevel.Debug, message, args);
        }

        public void Trace(string message, params object[] args)
        {
            logger.Trace(message, args);

            OnMessageWrited(logger.IsTraceEnabled, Interface.LogLevel.Trace, message, args);
        }

        private void OnMessageWrited(bool isEnabled, Interface.LogLevel logLevel, string message, params object[] args)
        {
            if (isEnabled)
            {
                var e = new Interface.MessageWritedEventArgs()
                {
                    LoggerName = logger.Name,
                    LogLevel = logLevel
                };

                try
                {
                    e.Message = string.Format(message, args);
                }
                catch
                {
                    e.Message = message;
                }

                Interface.MessageWritedEventHelper.OnMessageWrited(this, e);
            }

        }

        private ILogger logger;
    }
}
