using Npgsql.Internal;

using System.Runtime.CompilerServices;

namespace Npgsql.Tvp.Internal.Converters.Binders
{
    /// <summary>
    /// Stores the row value and
    /// its metadata for writing.
    /// </summary>
    /// 
    /// <param name="OID">
    /// Unique id identifying the data type in a given database (in pg_type).
    /// </param>
    /// 
    /// <param name="WriteState">
    /// Only for nested composite types.
    /// </param>
    internal record struct Writable(uint OID, int Size, IStrongBox Box, PgConverter Converter, object WriteState)
    { }
}