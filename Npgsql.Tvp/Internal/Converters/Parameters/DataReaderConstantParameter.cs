using Npgsql.Tvp.Internal.Converters.Buffers.Contracts;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;
using Npgsql.Tvp.Internal.Converters.Parameters.Contracts.Abstracts;

using Npgsql.Tvp.Internal.Metadata;

using System;
using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Parameters
{
    internal sealed class DataReaderConstantParameter(CompositeMetadata metadata, IDataReader value, int rows, IWritableBuffer buffer, Action<IParameterBinder, IDataRecord> binder) : AbstractParameter(metadata.OID, metadata.Columns, rows, buffer)
    {
        /// <inheritdoc/>
        public override int this[int row]
        {
            get
            {
                Bind();

                return base[row];
            }
        }

        /// <inheritdoc/>
        /// 
        /// <remarks>
        /// To handle constant-size rows, binding occurs per row upon
        /// size retrieval, enabling a column-count-sized buffer that
        /// gets overwritten on each bind.
        /// </remarks>
        protected override void Bind()
        {
            if (value.Read())
            {
                binder(this, value);
            }
        }
    }
}