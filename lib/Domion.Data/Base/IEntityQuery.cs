using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Data.Base
{
    public interface IEntityQuery<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>> where = null);
    }
}
