using Npgsql.Tvp.Internal.Converters.Models;

using System;
using System.Buffers;

namespace Npgsql.Tvp.Internal.Converters.Buffers
{
    internal sealed class LinearValueBuffer(int columnsCount, int rowsCount) : IDisposable
    {
        private int _written = default;

        private int _rSize = default;
        private int _tSize = default;

        /// <summary>
        /// The row values.
        /// </summary>
        private Value[] _items = ArrayPool<Value>.Shared.Rent(columnsCount * rowsCount);

        /// <summary>
        /// The sizes of the rows.
        /// </summary>
        private int[] _sizes = ArrayPool<int>.Shared.Rent(rowsCount);

        /// <summary>
        /// The 
        /// amount of values written 
        /// to the underlying buffer 
        /// so far.
        /// </summary>
        public int Written
        {
            get => _written;
        }

        /// <summary>
        /// The total size of all rows.
        /// </summary>
        public int Size
        {
            get => _tSize;
        }

        public int this[int row]
        {
            get => _sizes[row];
        }

        /// <summary>
        /// Gets the value at 
        /// the specified row 
        /// and column.
        /// </summary>
        public Value this[int row, int column]
        {
            get => _items[columnsCount * row + column];
        }

        /// <summary>
        /// Expands the buffer capacity.
        /// </summary>
        /// 
        /// <remarks>
        /// The array must be cleared prior to its return to the pool to avoid
        /// potential memory leaks. Necessary for reference-holding types only.
        /// </remarks>
        private static T[] Resize<T>(T[] source, bool clearArray = false)
        {
            T[] destination = ArrayPool<T>.Shared.Rent(source.Length * 2);

            Array.Copy(source, destination, source.Length);

            ArrayPool<T>.Shared.Return(source, clearArray);

            return destination;
        }

        /// <summary>
        /// Returns 
        /// true if the end of 
        /// the row is reached.
        /// </summary>
        private bool ShouldFlushSize()
        {
            return _written % columnsCount == 0;
        }

        /// <summary>
        /// Writes the row size to 
        /// the buffer and updates
        /// the total size.
        /// </summary>
        private void FlushSize()
        {
            _tSize += _rSize += sizeof(int) + columnsCount * Value.METADATA_SIZE;

            _sizes[_written / columnsCount - 1] = _rSize;

            _rSize = 0;
        }

        /// <inheritdoc cref="FlushSize"/>
        /// 
        /// <param name="size">
        /// -1 indicates a NULL column value.
        /// </param>
        private void WriteSize(int size)
        {
            if (size != Constants.NULL_SIZE)
            {
                _rSize += size;
            }

            if (ShouldFlushSize())
            {
                FlushSize();
            }
        }

        /// <summary>
        /// Writes the value to the buffer.
        /// </summary>
        public void Write(Value value)
        {
            if (_written == _items.Length)
            {
                _items = Resize(_items, true);
                _sizes = Resize(_sizes);
            }

            _items[_written++] = value;

            WriteSize(value.BufferRequirement.Value);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Array.Clear(_items, 0, _written);

            ArrayPool<Value>.Shared.Return(_items);
            ArrayPool<int>.Shared.Return(_sizes);
        }
    }
}