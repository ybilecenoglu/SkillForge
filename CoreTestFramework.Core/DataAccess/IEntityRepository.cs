using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Core.DataAccess
{
    //Database CRUD işlemlerini gerçekleştireceğimiz soyut katman T için IEntitiy interface türetilmeli ve newlenebilir olmalıdır.
    public interface IEntityRepository<T>  where T : IEntity, new()
    {
        Task<Result<List<T>>> GetListAsync(Expression<Func<T, bool>> filter = null);
        Result<IQueryable<T>> GetQueryable(Expression<Func<T, bool>> filter = null);
        Task<Result<T>> FindById(int id);
        Task<Result<T>> AddAsync(T entity);
        Task<Result<int>> AddRangeAsync(List<T> entity);
        Task<Result> DeleteAsync(T entity);
        Task<Result<int>> DeleteRangeAsync(List<T> entity);
        Task<Result<T>> UpdateAsync(T entity);
    }
}
