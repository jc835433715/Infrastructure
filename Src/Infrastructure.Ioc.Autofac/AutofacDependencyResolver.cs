using Autofac;
using Infrastructure.Ioc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Ioc.Autofac
{
    public class AutofacDependencyResolver : IDependencyResolver
    {
        public AutofacDependencyResolver(object dependencyConfigModule, object container = null)
        {
            this.container = container as IContainer;

            if (container == null)
            {
                ContainerBuilder builder = new ContainerBuilder();

                builder.RegisterModule(dependencyConfigModule as Module);

                this.container = builder.Build();
            }
        }

        public AutofacDependencyResolver(IEnumerable<string> dependencyConfigModuleAssemblyStrings, IEnumerable<string> dependencyConfigModuleNameSpaceStrings = null)
        {
            ContainerBuilder builder = new ContainerBuilder();
            var modules = new List<Module>();

            foreach (var dependencyConfigAssemblyString in dependencyConfigModuleAssemblyStrings)
            {
                System.Reflection.Assembly dependencyConfigAssembly = System.Reflection.Assembly.Load(dependencyConfigAssemblyString);
                var dependencyConfigs = GetAllModule(dependencyConfigAssembly, dependencyConfigModuleNameSpaceStrings);

                modules.AddRange(dependencyConfigs);
            }

            modules.ForEach(e => builder.RegisterModule(e));

            this.container = builder.Build();
        }
        
        public T GetService<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? container.Resolve<T>() : container.ResolveNamed<T>(name);
        }

        public object GetService(Type serviceType, string name = null)
        {
            return string.IsNullOrEmpty(name) ? container.Resolve(serviceType) : container.ResolveNamed(name, serviceType);
        }

        public IEnumerable<T> GetServices<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType, string name = null)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Module> GetAllModule(System.Reflection.Assembly dependencyConfigAssembly, IEnumerable<string> dependencyConfigNameSpaceStrings)
        {
            return from t in dependencyConfigAssembly.GetTypes()
                   where t.BaseType == typeof(DependencyConfigModule) && (dependencyConfigNameSpaceStrings == null || dependencyConfigNameSpaceStrings.Any(e => t.FullName.StartsWith(e)))
                   select dependencyConfigAssembly.CreateInstance(t.FullName) as Module;
        }

        private IContainer container;
    }
}
