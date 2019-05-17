using Infrastructure.Repository.Interface;
using System;
using System.Data.Entity;

namespace Infrastructure.Repository.EF
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork, IDisposable 
        where TContext : DbContext, new()
    {
        public UnitOfWorkBase()
        {
            Context = new TContext();
        }

        public TContext Context
        {
            get;
            set;
        }

        public abstract TRepository CreateRepository<TRepository, TAggregateRoot>()
            where TRepository : IRepository<TAggregateRoot> 
            where TAggregateRoot : class, IAggregateRoot;

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
