using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters.Buffers;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Abstracts
{
    internal abstract class AbstractParameter(string dataTypeName, int columnsCount, int rowsCount, PgSerializerOptions options) : IParameter
    {
        private readonly
            Lazy<ArrayValueBuffer> _buffer = new
            Lazy<ArrayValueBuffer>
            (() => new ArrayValueBuffer(columnsCount, rowsCount));

        protected ArrayValueBuffer Buffer
        {
            get => _buffer.Value;
        }

        /// <inheritdoc/>
        public int ColumnsCount
        {
            get => columnsCount;
        }

        /// <inheritdoc/>
        public int RowsCount
        {
            get => Buffer.Written / columnsCount;
        }

        /// <inheritdoc/>
        public int MetadataSize
        {
            get => sizeof(int) + sizeof(int) + sizeof(uint) + Constants.DIMENSIONS * (sizeof(int) + sizeof(int)) + sizeof(int) * RowsCount;
        }

        /// <inheritdoc/>
        public uint OID
        {
            get => options.GetArrayElementTypeId(new DataTypeName(dataTypeName).ToArrayName()).Oid.Value;
        }

        /// <inheritdoc/>
        public Value this[int row, int column]
        {
            get => Buffer[row, column];
        }

        /// <summary>
        /// Fills the buffer with parameter 
        /// values for subsequent efficient 
        /// and unified reuse.
        /// </summary>
        protected abstract void FillBuffer();

        /// <summary>
        /// 
        /// </summary>
        protected Value CreateValue(object value, Type type)
        {
            PgTypeInfo pgTypeInfo = options.GetDefaultTypeInfo(type);

            PgConverterResolution resolution = pgTypeInfo.GetObjectResolution(value);

            return new Value(value, resolution, pgTypeInfo.GetBufferRequirements(resolution.Converter, DataFormat.Binary).Value.Write);
        }

        /// <inheritdoc/>
        public int GetSize()
        {
            FillBuffer();

            return MetadataSize + Buffer.Size;
        }

        /// <inheritdoc/>
        public int GetRowSize(int row)
        {
            return Buffer[row];
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            Buffer.Dispose();
        }
    }
}