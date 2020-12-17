using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DND.EFWithNoLock.Extensions
{
    public static class EntityFrameworkExtenstions
    {
        private static TransactionScope CreateTrancationAsync()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                    new TransactionOptions()
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                    },
                                    TransactionScopeAsyncFlowOption.Enabled);
        }
        private static TransactionScope CreateTrancation()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                                    new TransactionOptions()
                                    {
                                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                    });
        }
        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTrancation())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }
        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTrancation())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancation())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                T result = query.FirstOrDefault();
                scope.Complete();
                return result;
            }
        }

        public static async Task<T> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancation())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                T result = await query.FirstOrDefaultAsync(cancellationToken);
                scope.Complete();
                return result;
            }
        }
        public static int CountWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancationAsync())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                int toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }
        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTrancationAsync())
            {
                if (expression != null)
                {
                    query = query.Where(expression);
                }
                int toReturn = await query.CountAsync(cancellationToken);
                scope.Complete();
                return toReturn;
            }
        }
    }
}
