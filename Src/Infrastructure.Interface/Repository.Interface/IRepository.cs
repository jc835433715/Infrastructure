using System;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interface
{
    /// <summary>
    /// 通用仓库接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    public interface IRepository<TAggregateRoot> : IQuery<TAggregateRoot>, IDisposable
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">聚合根</param>
        void Add(TAggregateRoot entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">聚合根</param>
        void AddRange(IEnumerable<TAggregateRoot> entities);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="entity">聚合根</param>
        void Remove(TAggregateRoot entity);

        /// <summary>
        /// 保存
        /// </summary>
        void Save();
    }
}
