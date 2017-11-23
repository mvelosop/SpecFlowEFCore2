using Domion.Lib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domion.Data.Base
{
    /// <summary>
    ///     Generic repository implementation.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        ///     Creates the generic repository instance.
        /// </summary>
        /// <param name="dbContext">The DbContext to get the Entity Type from.</param>
        protected BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected virtual DbContext DbContext => _dbContext;

        protected virtual DbSet<TEntity> DbSet => _dbSet;

        /// <summary>
        ///     Returns a query expression that, when enumerated, will retrieve all objects.
        /// </summary>
        protected virtual IQueryable<TEntity> Query()
        {
            return Query(null);
        }

        /// <summary>
        ///     Returns a query expression that, when enumerated, will retrieve only the objects that satisfy the where condition.
        /// </summary>
        protected virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query;
        }

        /// <summary>
        ///     Saves changes asynchronously from the DbContext's change tracker to the database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        protected virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if it passes the ValidateDelete method.
        /// </summary>
        protected virtual async Task<List<ValidationResult>> TryDeleteAsync(TEntity entity)
        {
            List<ValidationResult> errors = await ValidateDeleteAsync(entity);

            if (errors.Any())
            {
                return errors;
            }

            _dbSet.Remove(entity);

            return Errors.NoError;
        }

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual async Task<List<ValidationResult>> TryInsertAsync(TEntity entity)
        {
            List<ValidationResult> errors = await ValidateSaveAsync(entity);

            if (errors.Any())
            {
                return errors;
            }

            _dbSet.Add(entity);

            return Errors.NoError;
        }

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual async Task<List<ValidationResult>> TryUpdateAsync(TEntity entity)
        {
            List<ValidationResult> errors = await ValidateSaveAsync(entity);

            if (errors.Any())
            {
                return errors;
            }

            _dbSet.Update(entity);

            return Errors.NoError;
        }

        /// <summary>
        ///     Validates if it's ok to delete the entity from the database.
        /// </summary>
        protected virtual async Task<List<ValidationResult>> ValidateDeleteAsync(TEntity model)
        {
            return Errors.NoError;
        }

        /// <summary>
        ///     Validates if it's ok to save the new or updated entity to the database.
        /// </summary>
        protected virtual async Task<List<ValidationResult>> ValidateSaveAsync(TEntity model)
        {
            return Errors.NoError;
        }
    }
}
