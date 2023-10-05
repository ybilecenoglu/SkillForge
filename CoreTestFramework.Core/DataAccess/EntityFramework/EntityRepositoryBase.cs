using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreTestFramework.Core.DataAccess.EntityFramework
{
    //CRUD işlemlerini gerçekleştireceğimiz somut sınıf, IEntityRepository interface türetilmiştir TEntity için class olmalı IEntity interfaceden türetilmeli ve newlenebilir olmalıdır.
    //Standart CRUD işlemlerini gerçekleştirdiğimiz sınıf
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity,  new()
     where  TContext : DbContext, new()
    {
        public async Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            var add_result = new Result<TEntity> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    await db.AddAsync(entity);
                    await db.SaveChangesAsync();
                    add_result.Success = true;
                    add_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                add_result.Message = ex.Message;
            }

            return add_result;
        }
        public async Task<Result<int>> AddRangeAsync(List<TEntity> entity)
        {
            var add_range_result = new Result<int> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    await db.AddRangeAsync(entity);
                    await db.SaveChangesAsync();
                    add_range_result.Success = true;
                    add_range_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                add_range_result.Message = ex.Message;
            }

            return add_range_result;
        }
        public async Task<Result<int>> DeleteAsync(TEntity entity)
        {
            var delete_result = new Result<int> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    db.Remove(entity);
                    await db.SaveChangesAsync();

                    delete_result.Success = true;
                    delete_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                delete_result.Message = ex.Message;
            }
            return delete_result;
        }
        public async Task<Result<int>> DeleteRangeAsync(List<TEntity> entity)
        {
            var delete_range_result = new Result<int> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    db.RemoveRange(entity);
                    await db.SaveChangesAsync();
                    delete_range_result.Success = true;
                    delete_range_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                delete_range_result.Message = ex.Message;
            }

            return delete_range_result;
        }
        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            var update_result = new Result<TEntity> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    db.Update(entity);
                    await db.SaveChangesAsync();
                    update_result.Success = true;
                    update_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                update_result.Message = ex.Message;
            }
            return update_result;
        }
        public async Task<Result<TEntity>> FindById(object id)
        {
            var get_result = new Result<TEntity> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    get_result.Data = await db.Set<TEntity>().FindAsync(id);
                    get_result.Success = true;
                    get_result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                get_result.Message = ex.Message;
            }

            return get_result;
        }
        public async Task<Result<List<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var get_list_result = new Result<List<TEntity>> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    get_list_result.Data = filter != null ? await db.Set<TEntity>().Where(filter).ToListAsync() : await db.Set<TEntity>().ToListAsync();
                    get_list_result.Success = true;
                    get_list_result.Message = "Success";
                }

            }
            catch (Exception ex)
            {
                get_list_result.Message = ex.Message;
            }

            return get_list_result;
        }
        public Result<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null)
        {
            var queryable_result = new Result<IQueryable<TEntity>> { Success = false };
            try
            {
                using (var db = new TContext())
                {
                    queryable_result.Data = filter != null ? db.Set<TEntity>().Where(filter) : db.Set<TEntity>();
                    queryable_result.Success = true;
                    queryable_result.Message = "Success";
                }

            }
            catch (Exception ex)
            {
                queryable_result.Message = ex.Message;
            }

            return queryable_result;
        }

    }
}
