using System.Linq.Expressions;
using CoreTestFramework.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Core.Common
{
    public static class Tools 
    {
         public static IQueryable<T> LoadInclude<T>(IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T: class, IEntity
        {
            if (includes != null)
            {
               query =  includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}