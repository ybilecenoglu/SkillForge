using CoreTestFramework.Core.Common;
using CoreTestFramework.Core.Entities;
using Microsoft.EntityFrameworkCore;
using PostSharp.Aspects.Advices;
using System.Linq.Expressions;

namespace CoreTestFramework.Core.DataAccess.EntityFramework
{
    //CRUD işlemlerini gerçekleştireceğimiz somut sınıf, IEntityRepository interface türetilmiştir TEntity için class olmalı IEntity interfaceden türetilmeli ve newlenebilir olmalıdır.
    //Standart CRUD işlemlerini gerçekleştirdiğimiz sınıf
    public class EntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
     where TContext : DbContext, new()
    {
        public async Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            var add_result = new Result<TEntity> { Success = false };
            using (var db = new TContext())
                {
                    using (var transaction = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var added_entry = db.Entry(entity);
                            added_entry.State = EntityState.Added;
                            await db.SaveChangesAsync();
                            add_result.Success = true;
                            add_result.Message = "Success";
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            add_result.Message = ex.Message;
                            await transaction.RollbackAsync();
                        }
                    }
                }

            return add_result;
        }
        public async Task<Result<int>> AddRangeAsync(List<TEntity> entity)
        {
            var add_range_result = new Result<int> { Success = false };
            using (var db = new TContext())
                {
                    using (var transaction = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await db.AddRangeAsync(entity);
                            await db.SaveChangesAsync();
                            add_range_result.Success = true;
                            add_range_result.Message = "Success";
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            add_range_result.Message = ex.Message;
                            await transaction.RollbackAsync();
                        } 
                    }
                    
                }
            return add_range_result;
        }
        public async Task<Result> DeleteAsync(TEntity entity)
        {
            var delete_result = new Result { Success = false };
            using (var db = new TContext())
                {
                    using (var transaction = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var delete_entry = db.Entry(entity);
                            delete_entry.State = EntityState.Deleted;
                            await db.SaveChangesAsync();
                            delete_result.Success = true;
                            delete_result.Message = "Success";
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            delete_result.Message = ex.Message;
                            await transaction.RollbackAsync();
                        }
                    }
                }
            
            return delete_result;
        }
        public async Task<Result<int>> DeleteRangeAsync(List<TEntity> entity)
        {
            var delete_range_result = new Result<int> { Success = false };
            using (var db = new TContext())
                {
                    using (var transaction = await db.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            db.RemoveRange(entity);
                            await db.SaveChangesAsync();
                            delete_range_result.Success = true;
                            delete_range_result.Message = "Success";
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            delete_range_result.Message = ex.Message;
                            await transaction.RollbackAsync();
                        }
                    }
                }
            
            return delete_range_result;
        }
        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            var update_result = new Result<TEntity> { Success = false };
            using (var db = new TContext())
            {
                using (var transaction = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var update_entry = db.Entry(entity);
                        update_entry.State = EntityState.Modified;
                        await db.SaveChangesAsync();
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
