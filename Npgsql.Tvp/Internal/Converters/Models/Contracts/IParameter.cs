using System;

namespace Npgsql.Tvp.Internal.Converters.Models.Contracts
{
    internal interface IParameter : IDisposable
    {
        int ColumnsCount
        {
            get;
        }

        int RowsCount
        {
            get;
        }

        /// <summary>
        /// Size of the parameter headers.
        /// </summary>
        /// 
        /// <remarks>
        /// Dimensions + Flags + OID + $DIMENSIONS * (array length and lower bound) + $Value size integers.
        /// </remarks>
        int MetadataSize
        {
            get;
        }

        /// <summary>
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </summary>
        uint OID
        {
            get;
        }

        Value this[int row, int column]
        {
            get;
        }

        int GetSize();

        int GetRowSize(int row);
    }
}