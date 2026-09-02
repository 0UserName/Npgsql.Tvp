using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Metadata
{
    /// <summary>
    /// Stores user and database type metadata.
    /// </summary>
    internal readonly record struct ColumnMetadata(uint OID, bool AllowDBNull, Size Size, Type Type, PgConverter Converter)
    {
        public int GetSize<T>(T value, ref object writeState)
        {
            return AllowDBNull || Size.Kind != SizeKind.Exact ? Accessors.GetSizeOrDbNull(default, (PgConverter<T>)Converter, DataFormat.Binary, Size, value, ref writeState).Value.Value : Size.Value;
        }
    }
}