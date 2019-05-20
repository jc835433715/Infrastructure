using Infrastructure.Ioc.Interface;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Infrastructure.Ioc.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver, IDisposable
    {
        public NinjectDependencyResolver(object kernel = null)
        {
            this.kernel = (kernel as IKernel) ?? new StandardKernel();
        }

        public NinjectDependencyResolver(IEnumerable<string> dependencyConfigAssemblyStrings, IEnumerable<string> dependencyConfigNameSpaceStrings = null, object kernel = null)
            : this(kernel)
        {
            foreach (var dependencyConfigAssemblyString in dependencyConfigAssemblyStrings)
            {
                Assembly dependencyConfigAssembly = Assembly.Load(dependencyConfigAssemblyString);
                List<DependencyConfigBase> dependencyConfigList = GetAllNinjectModule(dependencyConfigAssembly, dependencyConfigNameSpaceStrings);

                dependencyConfigList.ForEach(e => e.Load(this.kernel));
            }
        }

        public NinjectDependencyResolver(object dependencyConfig, object kernel = null)
            : this(kernel)
        {
            (dependencyConfig as DependencyConfigBase).Load(this.kernel);
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

        public Lazy<T> GetLazyService<T>(string name = null)
        {
            return new Lazy<T>((Func<T>)(() => this.GetService<T>(name)));
        }

        public Lazy<IEnumerable<T>> GetLazyServices<T>(string name = null)
        {
            return new Lazy<IEnumerable<T>>((Func<IEnumerable<T>>)(() => this.GetServices<T>(name)));
        }

        private List<DependencyConfigBase> GetAllNinjectModule(Assembly dependencyConfigAssembly, IEnumerable<string> dependencyConfigNameSpaceStrings)
        {
            return (from t in dependencyConfigAssembly.GetTypes()
                    where t.BaseType == typeof(DependencyConfigBase) && (dependencyConfigNameSpaceStrings == null || dependencyConfigNameSpaceStrings.Any(e => t.FullName.StartsWith(e)))
                    select dependencyConfigAssembly.CreateInstance(t.FullName) as DependencyConfigBase).ToList();
        }

        public void Dispose()
        {
            kernel.Dispose();
        }

        private IKernel kernel;
    }
}
