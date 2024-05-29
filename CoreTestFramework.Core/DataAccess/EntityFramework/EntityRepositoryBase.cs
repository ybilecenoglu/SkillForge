using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PostSharp.Aspects.Advices;
using System.Linq.Expressions;

namespace CoreTestFramework.Core.DataAccess.EntityFramework
{
    //IEntitiyRepositoryBase interface'den miras alarak CRUD işlemlerini gerçekleştireceğimiz somut sınıf, TEntity için class olmalı IEntity interfaceden türetilmeli ve newlenebilir olmalıdır.
    //Standart CRUD işlemlerini gerçekleştirdiğimiz sınıf
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
     where TContext : DbContext, new()
    {
        //private readonly TContext _dbContext;
        public EntityRepositoryBase()
        {
            //_dbContext = new TContext();
        }
        public async Task<Result<TEntity>> AddAsync(TEntity entity) //Create operasyonları gerçekleştirdiğim asenktron method
        {
            var add_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {

                try
                {
                    var added_entry = _dbContext.Entry(entity);
                    added_entry.State = EntityState.Added;
                    await _dbContext.SaveChangesAsync();
                    add_result.Success = true;
                    add_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    add_result.Message = ex.Message;

                }
            }
            return add_result;
        }
        public async Task<Result<int>> AddRangeAsync(List<TEntity> entities)//Bir veya daha fazla insert operasyonları gerçekleştirdiğim asenktron method
        {
            var add_range_result = new Result<int> { Success = false };
            using (var _dbContext = new TContext())
            {

                try
                {
                    var added_entry = _dbContext.Entry(entities);
                    added_entry.State = EntityState.Added;
                    await _dbContext.SaveChangesAsync();
                    add_range_result.Success = true;
                    add_range_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    add_range_result.Message = ex.Message;
                }
            }


            return add_range_result;
        }
        public async Task<Result> DeleteAsync(TEntity entity)
        {
            var delete_result = new Result { Success = false };
            using (var _dbContext = new TContext())
            {

                try
                {
                    var delete_entry = _dbContext.Entry(entity);
                    delete_entry.State = EntityState.Deleted;
                    await _dbContext.SaveChangesAsync();
                    delete_result.Success = true;
                    delete_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    delete_result.Message = ex.Message;

                }
            }


            return delete_result;
        }
        public async Task<Result<int>> DeleteRangeAsync(List<TEntity> entities)
        {
            var delete_range_result = new Result<int> { Success = false };
            using (var _dbContext = new TContext())
            {

                try
                {
                    var delete_entry = _dbContext.Entry(entities);
                    delete_entry.State = EntityState.Deleted;
                    await _dbContext.SaveChangesAsync();
                    delete_range_result.Success = true;
                    delete_range_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    delete_range_result.Message = ex.Message;

                }
            }


            return delete_range_result;
        }
        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            var update_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {

                try
                {
                    var update_entry = _dbContext.Entry(entity);
                    update_entry.State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    update_result.Success = true;
                    update_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    update_result.Message = ex.Message;
                }
            }

            return update_result;
        }
        public async Task<Result<TEntity>> FindByIdAsync(int id)
        {
            var get_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {
                try
                {
                    get_result.Data = await _dbContext.Set<TEntity>().FindAsync(id);
                    get_result.Success = true;
                    get_result.Message = "Success";

                }
                catch (Exception ex)
                {
                    get_result.Message = ex.Message;
                }
            }

            return get_result;

        }
        public async Task<Result<List<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var get_list_result = new Result<List<TEntity>> { Success = false };
            using (var _dbContext = new TContext())
            {
                try
                {
                    var query = filter != null ? _dbContext.Set<TEntity>().Where(filter) : _dbContext.Set<TEntity>();
                    if (includes != null)
                    {
                        query = includes.Aggregate(query, (current, include) => current.Include(include));
                        get_list_result.Data = await query.ToListAsync();
                    }
                    else
                        get_list_result.Data = await query.ToListAsync();

                    get_list_result.Success = true;
                    get_list_result.Message = "Success";

                }
                catch (Exception ex)
                {
                    get_list_result.Message = ex.Message;
 
                }
            }
            return get_list_result;
        }
        public Result<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable_result = new Result<IQueryable<TEntity>> { Success = false };
            using (var _dbContext = new TContext())
            {
                try
                {
                    queryable_result.Data = filter != null ? _dbContext.Set<TEntity>().Where(filter) : _dbContext.Set<TEntity>();
                    queryable_result.Success = true;
                    queryable_result.Message = "Success";
                }
                catch (Exception ex)
                {
                    queryable_result.Message = ex.Message;
                }
                return queryable_result;
            }

        }

        public async Task<Result<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var get_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {
                try
                {
                    var query = filter != null ? _dbContext.Set<TEntity>().Where(filter) : _dbContext.Set<TEntity>();
                    if (includes != null)
                    {
                        query = includes.Aggregate(query, (current, include) => current.Include(include));//Aggreagate bir dizi üzerinde biriktirici işlevi uygular
                        get_result.Data = await query.FirstAsync();
                    }
                    else
                        get_result.Data = await query.FirstAsync();

                    get_result.Success = true;
                    get_result.Message = "Success";
                }
                catch (System.Exception ex)
                {
                    get_result.Message = ex.Message;
                }
            }

            return get_result;
        }
    }
}
