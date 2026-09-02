using Npgsql.Tvp.Internal.Converters.Buffers.Contracts;

using Npgsql.Tvp.Internal.Converters.Contracts.Abstracts;

using Npgsql.Tvp.Internal.Converters.Parameters;
using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using Npgsql.Tvp.Internal.Metadata;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DataReaderConverter(CompositeMetadata metadata, Func<int, IWritableBuffer> bufferBuilder, Action<IParameterBinder, IDataRecord> binder) : AbstractConverter<IDataReader>
    {
        /// <inheritdoc/>
        protected override IParameter CreateParameter(IDataReader value)
        {
            return metadata.IsVariable ?

                new DataReaderVariableParameter(metadata, value, value.RecordsAffected, bufferBuilder(value.RecordsAffected), binder) :
                new DataReaderConstantParameter(metadata, value, value.RecordsAffected, bufferBuilder(value.RecordsAffected), binder);
        }
    }
}