using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repository.Interface
{
    /// <summary>
    /// 查询接口
    /// </summary>
    /// <typeparam name="TAggregateRoot">聚合根类型</typeparam>
    public interface IQuery<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 获取查询接口
        /// </summary>
        /// <returns>查询接口</returns>
        IQueryable<TAggregateRoot> GetQuery();
    }
}
