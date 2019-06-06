using Infrastructure.Ioc.Interface;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.Ioc.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        public NinjectDependencyResolver(object dependencyConfigModule, object kernel = null)
        {
            this.kernel = (kernel as IKernel) ?? new StandardKernel(dependencyConfigModule as INinjectModule);
        }

        public NinjectDependencyResolver(IEnumerable<string> dependencyConfigModuleAssemblyStrings, IEnumerable<string> dependencyConfigModuleNameSpaceStrings = null)
        {
            var ninjectModules = new List<NinjectModule>();

            foreach (var dependencyConfigAssemblyString in dependencyConfigModuleAssemblyStrings)
            {
                Assembly dependencyConfigAssembly = Assembly.Load(dependencyConfigAssemblyString);
                var dependencyConfigs = GetAllModule(dependencyConfigAssembly, dependencyConfigModuleNameSpaceStrings);

                ninjectModules.AddRange(dependencyConfigs);
            }

            this.kernel = new StandardKernel(ninjectModules.ToArray());
        }

        public T GetService<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? kernel.Get<T>() : kernel.Get<T>(name);
        }

        public IEnumerable<T> GetServices<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? kernel.GetAll<T>() : kernel.GetAll<T>(name);
        }

        public object GetService(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? kernel.Get(type) : kernel.Get(type, name);
        }

        public IEnumerable<object> GetServices(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? kernel.GetAll(type) : kernel.GetAll(type, name);
        }
        
        private IEnumerable<NinjectModule> GetAllModule(Assembly dependencyConfigAssembly, IEnumerable<string> dependencyConfigNameSpaceStrings)
        {
            return from t in dependencyConfigAssembly.GetTypes()
                   where t.BaseType == typeof(DependencyConfigModule ) && (dependencyConfigNameSpaceStrings == null || dependencyConfigNameSpaceStrings.Any(e => t.FullName.StartsWith(e)))
                   select dependencyConfigAssembly.CreateInstance(t.FullName) as NinjectModule;
        }
        
        private IKernel kernel;
    }
}
