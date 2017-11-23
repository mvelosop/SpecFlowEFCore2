using Domion.Data.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domion.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(
            this IEntityQuery<TEntity> repository) where TEntity : class
        {
            return await repository.Query().FirstOrDefaultAsync();
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(
            this IEntityQuery<TEntity> repository,
            Expression<Func<TEntity, bool>> where) where TEntity : class
        {
            return await repository.Query(where).FirstOrDefaultAsync();
        }

        public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IEntityQuery<TEntity> repository,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        {
            return repository.Query().Include(navigationPropertyPath);
        }
    }
}
