using Npgsql.Internal;

using System;
using System.Collections.Concurrent;

using System.Linq.Expressions;

using System.Runtime.CompilerServices;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters.Binders
{
    internal static class WritableBinder
    {
        private static readonly
            ConcurrentDictionary<uint, Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask>> _cache = new
            ConcurrentDictionary<uint, Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask>>
            ();

        private static readonly
            ParameterExpression[] _args = new
            ParameterExpression[]
            {
                Expression.Parameter(typeof(PgConverter)), Expression.Parameter(typeof(PgWriter)), Expression.Parameter(typeof(IStrongBox)), Expression.Parameter(typeof(CancellationToken))
            };

        private static Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask> CreateBinder(Type typeArg)
        {
            MethodCallExpression call = Expression.Call(default, typeof(ParameterWriter).GetMethod(nameof(ParameterWriter.WriteValueAsync)).MakeGenericMethod(typeArg), _args);

            Expression<Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask>> lambda = Expression.Lambda
                      <Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask>>
                      (call, _args);

            return lambda.Compile();
        }

        /// <param name="oid">
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </param>
        public static void Add(uint oid, Type typeArg)
        {
            _ = _cache.GetOrAdd(oid, static (o, t) => CreateBinder(t), typeArg);
        }

        /// <param name="oid">
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </param>
        public static Func<PgConverter, PgWriter, IStrongBox, CancellationToken, ValueTask> Get(uint oid)
        {
            return _cache[oid];
        }
    }
}