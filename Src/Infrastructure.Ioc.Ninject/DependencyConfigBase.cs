using Ninject;

namespace Infrastructure.Ioc.Ninject
{
    /// <summary>
    /// �������û���
    /// </summary>
    public abstract class DependencyConfigBase
    {
        public abstract void Load(IKernel kernel);
    }
}
