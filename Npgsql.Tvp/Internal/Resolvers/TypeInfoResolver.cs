using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class TypeInfoResolver : IPgTypeInfoResolver
    {
        private static PgResolverTypeInfo TryGetResolver<TParameter, TResolver>(Type type, PgSerializerOptions options) where TResolver : PgConverterResolver
        {
            return type.IsAssignableTo(typeof(TParameter)) ? new PgResolverTypeInfo(options, (TResolver)Activator.CreateInstance(typeof(TResolver), options), default, type) : default;
        }

        /// <inheritdoc/>
        public PgTypeInfo? GetTypeInfo(Type? type, DataTypeName? dataTypeName, PgSerializerOptions options)
        {
            return type == default ? default : TryGetResolver<DataTable, DataTableConverterResolver>(type, options) ?? TryGetResolver<IDataReader, DataReaderConverterResolver>(type, options);
        }
    }
}