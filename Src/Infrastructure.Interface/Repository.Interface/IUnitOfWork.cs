using System;

namespace Infrastructure.Repository.Interface
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 创建仓库
        /// </summary>
        /// <typeparam name="TRepository">仓库类型</typeparam>
        /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
        /// <returns>仓库</returns>
        TRepository CreateRepository<TRepository, TAggregateRoot>()
            where TRepository : IRepository<TAggregateRoot>
            where TAggregateRoot : class, IAggregateRoot;

        /// <summary>
        /// 保存
        /// </summary>
        void Save();
    }
}
