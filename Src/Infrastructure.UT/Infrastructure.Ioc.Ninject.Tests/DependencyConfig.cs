using System;

namespace Infrastructure.Ioc.Ninject.Tests
{
    class DependencyConfig : DependencyConfigModule 
    {
        public override void Load()
        {
            this.Kernel.Bind<ILogger>().To<FileLogger>().Named(LoggerType.File.ToString());

            this.Kernel.Bind<ILogger>().To<DbLogger>().Named(LoggerType.Db.ToString());

            this.Kernel.Bind<IA, IB>().To<AB>()
                .InSingletonScope();
        }
    }

    interface ILogger
    {
        void Write(string message);
    }

    enum LoggerType
    {
        File,
        Db
    }

    class FileLogger : ILogger
    {
        public void Write(string message)
        {
            throw new NotImplementedException();
        }
    }

    class DbLogger : ILogger
    {
        public void Write(string message)
        {
            throw new NotImplementedException();
        }
    }

    interface IA
    {
    }

    interface IB
    {
    }

    class AB : IA, IB
    {

    }
}
