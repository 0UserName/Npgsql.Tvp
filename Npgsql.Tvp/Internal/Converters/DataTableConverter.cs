using Npgsql.Tvp.Internal.Converters.Buffers.Contracts;

using Npgsql.Tvp.Internal.Converters.Contracts.Abstracts;

using Npgsql.Tvp.Internal.Converters.Parameters;
using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using Npgsql.Tvp.Internal.Metadata;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DataTableConverter(CompositeMetadata metadata, Func<int, IWritableBuffer> bufferBuilder, Action<IParameterBinder, IDataRecord> binder) : AbstractConverter<DataTable>
    {
        /// <inheritdoc/>
        protected override IParameter CreateParameter(DataTable value)
        {
            return metadata.IsVariable ?

                new DataReaderVariableParameter(metadata, new DataTableReader(value), value.Rows.Count, bufferBuilder(value.Rows.Count), binder) :
                new DataReaderConstantParameter(metadata, new DataTableReader(value), value.Rows.Count, bufferBuilder(value.Rows.Count), binder);
        }
    }
}