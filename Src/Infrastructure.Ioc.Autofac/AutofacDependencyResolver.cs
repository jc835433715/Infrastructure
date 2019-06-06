using Infrastructure.Ioc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Ioc.Autofac
{
    public class AutofacDependencyResolver : IDependencyResolver, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Lazy<T> GetLazyService<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public Lazy<IEnumerable<T>> GetLazyServices<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public T GetService<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType, string name = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetServices<T>(string name = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType, string name = null)
        {
            throw new NotImplementedException();
        }
    }
}
