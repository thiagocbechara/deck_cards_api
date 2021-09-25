using DeckAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeckAPI.Infra.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity: BaseEntity
    {
        Task<IEnumerable<TEntity>> GellAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task InsertAsync(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
    }
}
