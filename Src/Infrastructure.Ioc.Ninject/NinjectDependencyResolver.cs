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
        public NinjectDependencyResolver(object kernel = null) => this.kernel = (kernel as IKernel) ?? new StandardKernel();

        public void Initialize(IEnumerable<string> dependencyConfigAssemblyStrings)
        {
            var tasks = new List<Task>();

            foreach (var dependencyConfigAssemblyString in dependencyConfigAssemblyStrings)
            {
                var temp = dependencyConfigAssemblyString;

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    Assembly dependencyConfigAssembly = Assembly.Load(temp);
                    List<DependencyConfigBase> dependencyConfigList = GetAllNinjectModule(dependencyConfigAssembly);

                    dependencyConfigList.ForEach(e => e.Load(kernel));

                }, TaskCreationOptions.LongRunning));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public void InitializeForUT(object dependencyConfig) => (dependencyConfig as DependencyConfigBase).Load(kernel);

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

        private List<DependencyConfigBase> GetAllNinjectModule(Assembly dependencyConfigAssembly)
        {
            return (from t in dependencyConfigAssembly.GetTypes()
                    where t.BaseType == typeof(DependencyConfigBase)
                    select dependencyConfigAssembly.CreateInstance(t.FullName) as DependencyConfigBase).ToList();
        }

        public void Dispose()
        {
            kernel.Dispose();
        }

        private IKernel kernel;
    }
}
