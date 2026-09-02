using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Binders;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using Npgsql.Tvp.Internal.Metadata;

using Npgsql.Tvp.Internal.Resolvers.Contracts.Abstracts;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataTableConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<DataTable, DataTableConverter>(options)
    {
        private readonly
            DataReaderBinderBuilder _builder = new
            DataReaderBinderBuilder
            ();

        /// <inheritdoc/>
        protected override DataTable GetSchema(DataTable value)
        {
            return new DataTableReader(value).GetSchemaTable();
        }

        /// <inheritdoc/>
        protected override string GetTypeName(DataTable value)
        {
            return value.TableName;
        }

        /// <inheritdoc/>
        protected override Action<IParameterBinder, IDataRecord> CompileBinder(CompositeMetadata metadata)
        {
            return _builder.Compile(metadata);
        }
    }
}