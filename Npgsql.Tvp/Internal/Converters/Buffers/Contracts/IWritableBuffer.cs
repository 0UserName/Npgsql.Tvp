using Npgsql.Tvp.Internal.Converters.Binders;

using System;

namespace Npgsql.Tvp.Internal.Converters.Buffers.Contracts
{
    internal interface IWritableBuffer : IDisposable
    {
        /// <summary>
        /// Gets the total size of all row values.
        /// </summary>
        int Size
        {
            get;
        }

        /// <summary>
        /// Gets the size of the values in the specified row.
        /// </summary>
        int this[int row]
        {
            get;
        }

        ref Writable GetMoveNext();
    }
}