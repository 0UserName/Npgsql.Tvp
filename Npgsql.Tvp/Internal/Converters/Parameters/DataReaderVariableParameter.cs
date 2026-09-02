using Npgsql.Tvp.Internal.Converters.Buffers.Contracts;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;
using Npgsql.Tvp.Internal.Converters.Parameters.Contracts.Abstracts;

using Npgsql.Tvp.Internal.Metadata;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Parameters
{
    internal sealed class DataReaderVariableParameter(CompositeMetadata metadata, IDataReader value, int rows, IWritableBuffer buffer, Action<IParameterBinder, IDataRecord> binder) : AbstractParameter(metadata.OID, metadata.Columns, rows, buffer)
    {
        /// <inheritdoc/>
        /// 
        /// <remarks>
        /// To handle variable row sizes and
        /// forward-only reading, all values
        /// are bound in one pass.
        /// </remarks>
        protected override void Bind()
        {
            while (value.Read())
            {
                binder(this, value);
            }
        }

        /// <inheritdoc/>
        public override int CalculateSize()
        {
            Bind();

            return base.CalculateSize();
        }
    }
}