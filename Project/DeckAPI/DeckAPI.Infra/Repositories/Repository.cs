using DeckAPI.Domain.Entities;
using DeckAPI.Infra.Db;
using DeckAPI.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeckAPI.Infra.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // GetAll
        // list.Where(x => x.Id > 0);
        public async Task<IEnumerable<TEntity>> GellAllAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return await _dbSet.ToListAsync();
            var query = _dbSet.Where(predicate);
            return await Include(query).ToListAsync();
        }

        /// <summary>
        /// Referencia: https://stackoverflow.com/a/44683480
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<TEntity> Include(IQueryable<TEntity> query)
        {
            var navigations = _context.Model.FindEntityType(typeof(TEntity))
                .GetDerivedTypesInclusive()
                .SelectMany(type => type.GetNavigations())
                .Distinct();

            foreach (var property in navigations)
                query = query.Include(property.Name);

            return query;
        }

        // Get
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            await Include(_dbSet.AsQueryable()).FirstOrDefaultAsync(predicate);

        // Insert
        public async Task InsertAsync(TEntity entity) =>
            await _dbSet.AddAsync(entity);

        // Update
        public void Update(TEntity entity) =>
            _dbSet.Update(entity);

        // Delete
        public void Delete(TEntity entity) =>
            _dbSet.Remove(entity);
    }
}
