using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Binders;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using Npgsql.Tvp.Internal.Metadata;

using Npgsql.Tvp.Internal.Resolvers.Contracts.Abstracts;

using System;
using System.Data;
using System.Data.Common;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DataReaderConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<IDataReader, DataReaderConverter>(options)
    {
        private readonly
            DataReaderBinderBuilder _builder = new
            DataReaderBinderBuilder
            ();

        /// <inheritdoc/>
        protected override DataTable GetSchema(IDataReader value)
        {
            return value.GetSchemaTable();
        }

        /// <inheritdoc/>
        protected override string GetTypeName(IDataReader value)
        {
            return (string)value.GetSchemaTable().Columns[SchemaTableColumn.BaseTableName].DefaultValue;
        }

        /// <inheritdoc/>
        protected override Action<IParameterBinder, IDataRecord> CompileBinder(CompositeMetadata metadata)
        {
            return _builder.Compile(metadata);
        }
    }
}