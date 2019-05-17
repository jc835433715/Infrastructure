using Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repository.EF
{
    public abstract class RepositoryBase<TAggregateRoot, TContext> : IRepository<TAggregateRoot>, IQuery<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
        where TContext : DbContext, new()
    {
        protected RepositoryBase()
        {
            this.Context = new TContext();
        }

        public TContext Context
        {
            get;
            set;
        }

        protected IDbSet<TAggregateRoot> DbSet
        {
            get
            {
                return Context.Set<TAggregateRoot>();
            }
        }

        public IQueryable<TAggregateRoot> GetQuery()
        {
            return DbSet;
        }

        public IEnumerable<TAggregateRoot> Find(Expression<Func<TAggregateRoot, bool>> where)
        {
            return DbSet.Where(where);
        }

        public void Add(TAggregateRoot entity)
        {
            DbSet.Add(entity);
        }

        public void AddRange(IEnumerable<TAggregateRoot> entities)
        {
            bool autoDetectChangesEnabled = Context.Configuration.AutoDetectChangesEnabled;
            bool validateOnSaveEnabled = Context.Configuration.ValidateOnSaveEnabled;

            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                Context.Configuration.ValidateOnSaveEnabled = false;

                entities.ToList().ForEach(delegate (TAggregateRoot e)
               {
                   Add(e);
               });
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled;
                Context.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
            }
        }

        public void Remove(TAggregateRoot entity)
        {
            DbSet.Remove(entity);
        }

        public void Save()
        {
            TContext context = Context;

            context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
