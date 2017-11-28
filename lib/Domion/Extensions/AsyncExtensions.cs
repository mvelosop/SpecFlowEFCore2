using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Extensions
{
    public static class AsyncQueryableExtensions
    {
        public static AsyncQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> enumerable)
        {
            return new AsyncQueryable<T>(enumerable);
        }
    }

    public class AsyncQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly IAsyncEnumerable<T> _asyncEnumerable;
        private readonly List<T> _list;
        private readonly IQueryable<T> _queryable;

        public AsyncQueryable(
            IEnumerable<T> enumerable)
        {
            _list = enumerable.ToList();

            _queryable = _list.AsQueryable();
            _asyncEnumerable = _list.ToAsyncEnumerable();
        }

        public Type ElementType => _queryable.ElementType;

        public Expression Expression => _queryable.Expression;

        public IQueryProvider Provider => _queryable.Provider;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator() => _asyncEnumerable.GetEnumerator();
    }
}
