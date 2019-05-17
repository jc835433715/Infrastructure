using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Ioc.Ninject;
using Ninject;

namespace Infrastructure.Ioc.Ninject.Tests
{
    class DependencyConfig : DependencyConfigBase
    {
        public override void Load(IKernel kernel)
        {
            kernel.Bind<ILogger>().To<FileLogger>().Named(LoggerType.File.ToString());

            kernel.Bind<ILogger>().To<DbLogger>().Named(LoggerType.Db.ToString());

            kernel.Bind<IA, IB>().To<AB>()
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
