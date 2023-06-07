using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbstractModel.EFCore
{
    public class AbstractContext<TEntity> : IAbstractContext<TEntity> where TEntity : class, new()
    {
        public DbContext _context { get; set; }

        public DbSet<TEntity> DbSet { get; set; }

        public AbstractContext() { }

        public AbstractContext(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual IEnumerable<TEntity> GetAll(int limit, int index, out int total, out int totalPages)
        {
            total = DbSet.Count();
            totalPages = (int)Math.Ceiling((double)total / limit);

            return DbSet.Skip((index - 1) * limit)
                        .Take(limit)
                        .ToList();
        }

        public virtual IEnumerable<TEntity> GetAll(int limit, int index, out int total, out int totalPages, Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate.ToString().Equals("f => False"))
            {
                total = DbSet.Count();
                totalPages = (int)Math.Ceiling((double)total / limit);

                return _context.Set<TEntity>()
                    .Skip((index - 1) * limit)
                    .Take(limit)
                    .ToList();
            }

            total = DbSet.Where(predicate).Count();
            totalPages = (int)Math.Ceiling((double)total / limit);

            return _context.Set<TEntity>()
                    .Where(predicate)
                    .Skip((index - 1) * limit)
                    .Take(limit)
                    .ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties != null && includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return predicate.ToString().Equals("f => False") ? await query.ToListAsync() : await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate.ToString().Equals("f => False") ? await _context.Set<TEntity>().ToListAsync() : await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetAll(int limit, int index, out int total, out int totalPages, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties != null && includeProperties.Length > 0)
            {
                foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (predicate != null && !predicate.ToString().Equals("f => False"))
            {
                query = query.Where(predicate);
            }

            total = query.Count();
            totalPages = limit > 0 && index > 0 ? (int)Math.Ceiling((double)total / limit) : 1;

            return limit > 0 && index > 0 ? query.Skip((index - 1) * limit).Take(limit).ToList() : query.ToList();
        }

        public virtual async Task<TEntity> Get<TKey>(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate.ToString().Equals("f => False") ? await _context.Set<TEntity>().FirstOrDefaultAsync() : await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return predicate.ToString().Equals("f => False") ? await query.FirstOrDefaultAsync() : await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<TEntity>> FindLogBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return predicate.ToString().Equals("f => False") ? await query.ToListAsync() : await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await Save();
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Save();
            return entity;
        }

        public virtual async Task<bool> Remove(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Remove(entity);
            await Save();

            return true;
        }

        public virtual async Task<bool> RemoveWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = predicate.ToString().Equals("f => False") ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(predicate);

            foreach (TEntity entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }

            await Save();

            return true;
        }

        private async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
