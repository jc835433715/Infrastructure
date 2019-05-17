using Ninject;

namespace Infrastructure.Ioc.Ninject
{
    /// <summary>
    /// ÒÀÀµÅäÖÃ»ùÀà
    /// </summary>
    public abstract class DependencyConfigBase
    {
        public abstract void Load(IKernel kernel);
    }
}
