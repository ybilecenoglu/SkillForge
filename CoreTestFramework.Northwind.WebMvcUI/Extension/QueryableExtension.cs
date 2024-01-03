using System.Collections;
using System.Linq.Expressions;
using CoreTestFramework.Core.Entities;
using CoreTestFramework.Northwind.Entities.Model;
using DataTables.AspNet.Core;
using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Northwind.WebMvcUI.Extension
{
    public static class QueryableExtension
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<IColumn> sortModels)
        {
            var expression = source.Expression; //IQueryable örneğiyle ilişkili ifade ağacını alır.
            int count = 0;
            foreach (var item in sortModels)
            {
                var parameter = Expression.Parameter(typeof(T), "x"); //İfade ağacındaki parametreyi veya değişkeni tanımlamak için kullanılabilecek bir ParametreExpression düğümü oluşturur.
                
                Expression selector;
                if (item.Name.Contains("."))
                {
                    Expression body = parameter;
                    foreach (var member in item.Name.Split('.'))
                    {
                        body = Expression.PropertyOrField(body, member);
                    }
                    selector = body;
                }
                else {
                    selector = Expression.PropertyOrField(parameter, item.Field);
                }
                var method = item.Sort.Direction == SortDirection.Descending ? 
                (count == 0 ? "OrderByDescending" : "ThenByDescending"):
                (count == 0 ? "OrderBy": "ThenBy");
                expression = Expression.Call(typeof(Queryable), method, new Type[] {source.ElementType, selector.Type},expression, Expression.Quote(Expression.Lambda(selector,parameter)));
                count++;
            }
            
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}