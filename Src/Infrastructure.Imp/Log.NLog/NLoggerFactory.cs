

using NLog;

namespace Infrastructure.Log.NLog
{
    public class NLoggerFactory : Infrastructure.Log.Interface.ILoggerFactory
    {
        public Infrastructure.Log.Interface.ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T).FullName);
        }

        public Infrastructure.Log.Interface.ILogger GetLogger(string name)
        {
            Logger logger = LogManager.GetLogger(name);

            return new NLogger(logger);
        }
    }
}
