using SkillForge.Core.Common;
using SkillForge.Core.Entities;
using System.Linq.Expressions;

namespace SkillForge.Core.DataAccess
{
    //Database CRUD işlemlerini gerçekleştireceğimiz soyut katman T için IEntitiy interface türetilmeli ve newlenebilir olmalıdır.
    public interface IEntityRepository<T>  where T : IEntity, new()
    {
        Task<Result<List<T>>> GetListAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Result<IQueryable<T>> GetQueryable(Expression<Func<T, bool>> filter = null,params Expression<Func<T, object>>[] includes);
        Task<Result<T>> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
        Task<Result<T>> FindByIdAsync(int id);
        Task<Result<T>> AddAsync(T entity);
        Task<Result<int>> AddRangeAsync(List<T> entity);
        Task<Result> DeleteAsync(T entity);
        Task<Result<int>> DeleteRangeAsync(List<T> entity);
        Task<Result<T>> UpdateAsync(T entity);
        
    }
}
