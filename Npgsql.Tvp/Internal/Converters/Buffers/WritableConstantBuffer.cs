using Npgsql.Tvp.Internal.Converters.Binders;

using Npgsql.Tvp.Internal.Converters.Buffers.Contracts.Abstracts;

using System.Buffers;

namespace Npgsql.Tvp.Internal.Converters.Buffers
{
    internal sealed class WritableConstantBuffer(int columns, int rows, int size) : AbstractWritableBuffer(ArrayPool<Writable>.Shared, columns)
    {
        /// <inheritdoc/>
        public override int Size
        {
            get => rows * size;
        }

        /// <inheritdoc/>
        public override int this[int row]
        {
            get => size;
        }
    }
}