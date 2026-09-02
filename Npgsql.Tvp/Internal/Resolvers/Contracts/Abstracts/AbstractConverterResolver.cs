using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.PostgresTypes;

using Npgsql.Tvp.Internal.Converters.Buffers;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using Npgsql.Tvp.Internal.Metadata;

using System;
using System.Collections.Concurrent;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace Npgsql.Tvp.Internal.Resolvers.Contracts.Abstracts
{
    internal abstract class AbstractConverterResolver<TParameter, TConverter>(PgSerializerOptions options) : PgConverterResolver<TParameter> where TConverter : PgConverter
    {
        private readonly
            ConcurrentDictionary<uint, PgConverterResolution> _cache = new
            ConcurrentDictionary<uint, PgConverterResolution>
            ();

        private PgConverterResolution Factory(uint oid, CompositeMetadata metadata)
        {
            return new PgConverterResolution((PgConverter)Activator.CreateInstance(typeof(TConverter), metadata, metadata.IsVariable ?

                (int rows) => new WritableVariableBuffer(metadata.Columns.Length, rows) :
                (int rows) => new WritableConstantBuffer(metadata.Columns.Length, rows, metadata.Columns.Sum(c => c.Size.Value)), CompileBinder(metadata)), new PgTypeId(oid));
        }

        /// <summary>
        /// Returns the table that describes the columns metadata.
        /// </summary>
        /// 
        /// <remarks>
        /// Must provide at least <see cref="SchemaTableColumn.AllowDBNull"/> and <see cref="SchemaTableColumn.DataType"/>.
        /// </remarks>
        protected abstract DataTable GetSchema(TParameter value);

        /// <summary>
        /// Returns the data type name.
        /// </summary>
        protected abstract string GetTypeName(TParameter value);

        /// <summary>
        /// Compiles a binder for
        /// source values and the
        /// parameter.
        /// </summary>
        protected abstract Action<IParameterBinder, IDataRecord> CompileBinder(CompositeMetadata metadata);

        /// <inheritdoc/>
        public override PgConverterResolution GetDefault(PgTypeId? pgTypeId)
        {
            throw new NotSupportedException("Not supported");
        }

        /// <inheritdoc/>
        public override PgConverterResolution? Get(TParameter value, PgTypeId? expectedPgTypeId)
        {
            return Accessors.get_DatabaseInfo(options).GetPostgresType(GetTypeName(value)) is PostgresCompositeType type ? _cache.TryGetValue(type.Array.OID, out PgConverterResolution resolution) ? resolution : _cache.GetOrAdd(type.Array.OID, Factory, new CompositeMetadata(GetSchema(value), type, options)) : throw new ArgumentException("The provided type is not composite");
        }
    }
}