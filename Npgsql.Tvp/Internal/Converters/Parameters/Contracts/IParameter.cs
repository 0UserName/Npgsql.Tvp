using Npgsql.Tvp.Internal.Converters.Binders;

using System;

namespace Npgsql.Tvp.Internal.Converters.Parameters.Contracts
{
    internal interface IParameter : IDisposable
    {
        int Columns
        {
            get;
        }

        int Rows
        {
            get;
        }

        /// <summary>
        /// Gets the size of the parameter headers.
        /// </summary>
        /// 
        /// <remarks>
        /// Dimensions + Flags + OID + (array length and lower bound) * $DIMENSIONS + $Value size integers.
        /// </remarks>
        int MetadataSize
        {
            get;
        }

        /// <summary>
        /// Gets the unique id identifying the data type in a given database (in pg_type).
        /// </summary>
        uint OID
        {
            get;
        }

        /// <summary>
        /// Gets the size of the specified row.
        /// </summary>
        int this[int row]
        {
            get;
        }

        /// <summary>
        /// Calculates the total parameter size.
        /// </summary>
        int CalculateSize();

        ref Writable GetMoveNext();
    }
}