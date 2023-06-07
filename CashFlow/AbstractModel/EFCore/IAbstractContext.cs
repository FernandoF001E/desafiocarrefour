using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AbstractModel.EFCore
{
    public interface IAbstractContext<TEntity>
    {
        IQueryable<TEntity> GetAll();

        IEnumerable<TEntity> GetAll(int limit, int index, out int total, out int totalPages);

        IEnumerable<TEntity> GetAll(int limit, int index, out int total, out int totalPages, Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Get<TKey>(TKey id);

        Task<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Add(TEntity entity);

        Task<TEntity> Update(TEntity entity);

        Task<bool> Remove(TEntity entity);
    }
}
