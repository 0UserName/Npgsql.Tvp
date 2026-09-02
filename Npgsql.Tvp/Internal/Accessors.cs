using Npgsql.Internal;

using System.Runtime.CompilerServices;

namespace Npgsql.Tvp.Internal
{
    internal static class Accessors
    {
        /// <summary>
        /// Accessor for:
        /// 
        /// <code>
        /// internal NpgsqlDatabaseInfo DatabaseInfo
        /// {
        ///     get;
        /// }
        /// </code>
        /// 
        /// </summary>
        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = nameof(get_DatabaseInfo))]
        public extern static NpgsqlDatabaseInfo get_DatabaseInfo(PgSerializerOptions options);

        /// <summary>
        /// Accessor for:
        /// 
        /// <code>
        /// internal static class PgConverterExtensions
        /// {
        ///     public static Size? GetSizeOrDbNull<T>(this PgConverter<T> converter, DataFormat format, Size writeRequirement, T? value, ref object? writeState)
        /// }
        /// </code>
        /// 
        /// </summary>
        /// 
        /// <param name="this">
        /// Always null.
        /// </param>
        /// 
        /// <param name="format">
        /// Always <see cref="DataFormat.Binary"/>.
        /// </param>
        /// 
        /// <param name="writeState">
        /// Always null.
        /// </param>
        [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = nameof(GetSizeOrDbNull))]
        public extern static Size? GetSizeOrDbNull<T>([UnsafeAccessorType("Npgsql.Internal.PgConverterExtensions, Npgsql")] object @this, PgConverter<T> converter, DataFormat format, Size writeRequirement, T? value, ref object? writeState);
    }
}