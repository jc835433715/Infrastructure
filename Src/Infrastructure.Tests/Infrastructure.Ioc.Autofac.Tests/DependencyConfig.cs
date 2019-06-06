using System;
using Autofac;
using Autofac.Core;

namespace Infrastructure.Ioc.Autofac.Tests
{
    class DependencyConfig : DependencyConfigModule
    {
        protected override void Load(ContainerBuilder builde)
        {
            builde.RegisterType<FileLogger>().As<ILogger>().Named<ILogger>(LoggerType.File.ToString());

            builde.RegisterType<DbLogger>().As<ILogger>().Named<ILogger>(LoggerType.Db.ToString());

            builde.RegisterType<AB>().As<IA>().As<IB>()
                .SingleInstance();
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
