using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PostSharp.Aspects.Advices;
using System.Linq.Expressions;

namespace CoreTestFramework.Core.DataAccess.EntityFramework
{
    //CRUD işlemlerini gerçekleştireceğimiz somut sınıf, IEntityRepository interface türetilmiştir TEntity için class olmalı IEntity interfaceden türetilmeli ve newlenebilir olmalıdır.
    //Standart CRUD işlemlerini gerçekleştirdiğimiz sınıf
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
     where TContext : DbContext, new()
    {
        //private readonly TContext _northwindContext;
        public EntityRepositoryBase()
        {
            //_northwindContext = new TContext();
        }
        public async Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            var add_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {
                using (var _transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var added_entry = _dbContext.Entry(entity);
                        added_entry.State = EntityState.Added;
                        await _dbContext.SaveChangesAsync();
                        add_result.Success = true;
                        add_result.Message = "Success";
                        await _transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        add_result.Message = ex.Message;
                        await _transaction.RollbackAsync();
                    }
                }
            }

            return add_result;
        }
        public async Task<Result<int>> AddRangeAsync(List<TEntity> entity)
        {
            var add_range_result = new Result<int> { Success = false };
            using (var _dbContext = new TContext())
            {
                using (var _transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var added_entry = _dbContext.Entry(entity);
                        added_entry.State = EntityState.Added;
                        await _dbContext.SaveChangesAsync();
                        add_range_result.Success = true;
                        add_range_result.Message = "Success";
                        await _transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        add_range_result.Message = ex.Message;
                        await _transaction.RollbackAsync();
                    }
                }
            }

            return add_range_result;
        }
        public async Task<Result> DeleteAsync(TEntity entity)
        {
            var delete_result = new Result { Success = false };
            using (var _dbContext = new TContext())
            {
                using (var _transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var delete_entry = _dbContext.Entry(entity);
                        delete_entry.State = EntityState.Deleted;
                        await _dbContext.SaveChangesAsync();
                        delete_result.Success = true;
                        delete_result.Message = "Success";
                        await _transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        delete_result.Message = ex.Message;
                        await _transaction.RollbackAsync();
                    }
                }
            }

            return delete_result;
        }
        public async Task<Result<int>> DeleteRangeAsync(List<TEntity> entity)
        {
            var delete_range_result = new Result<int> { Success = false };
            using (var _dbContext = new TContext())
            {
                using (var _transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var delete_entry = _dbContext.Entry(entity);
                        delete_entry.State = EntityState.Deleted;
                        await _dbContext.SaveChangesAsync();
                        delete_range_result.Success = true;
                        delete_range_result.Message = "Success";
                        await _transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        delete_range_result.Message = ex.Message;
                        await _transaction.RollbackAsync();
                    }
                }
            }

            return delete_range_result;
        }
        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            var update_result = new Result<TEntity> { Success = false };
            using (var _dbContext = new TContext())
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var update_entry = _dbContext.Entry(entity);
                        update_entry.State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                        update_result.Success = true;
                        update_result.Message = "Success";
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        update_result.Message = ex.Message;
                        await transaction.RollbackAsync();
                    }
                }
            }
            return update_result;
        }
        public async Task<Result<TEntity>> FindById(int id)
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
        public async Task<Result<List<TEntity>>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var get_list_result = new Result<List<TEntity>> { Success = false };
            using (var _dbContext = new TContext())
            {
                try
                {
                    get_list_result.Data = filter != null ? await _dbContext.Set<TEntity>().Where(filter).AsNoTracking().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
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
        public Result<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null)
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
            }
            return queryable_result;
        }
    }
}
