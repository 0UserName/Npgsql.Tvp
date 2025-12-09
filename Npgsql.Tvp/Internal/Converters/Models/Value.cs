using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object value, PgConverterResolution resolution, Size writeRequirement)
    {
        private readonly (Size BufferRequirement, object WriteState) _state = GetSizeOrDbNullAsObject(value, resolution.Converter, writeRequirement);

        /// <summary>
        /// Size of the target value headers.
        /// </summary
        /// 
        /// <remarks>
        /// OID + Size.
        /// </remarks>
        public const int METADATA_SIZE = sizeof(uint) + sizeof(int);

        /// <summary>
        /// Target value.
        /// </summary>
        public object UnderlyingValue
        {
            get => value;
        }

        /// <summary>
        /// Unique id identifying the data type in a given database (in pg_type).
        /// </summary>
        public uint OID
        {
            get;

        } = resolution.PgTypeId.Oid.Value;

        /// <summary>
        /// Handles the logic for 
        /// processing the target 
        /// value.
        /// </summary>
        public PgConverter Converter
        {
            get;

        } = resolution.Converter;

        public Size BufferRequirement
        {
            get => _state.BufferRequirement;
        }

        /// <summary>
        /// Not really used.
        /// </summary>
        public object WriteState
        {
            get => _state.WriteState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// GetDefaultTypeInfo 
        /// returns a non-nullable converter because DataTable does not support 
        /// nullable column types. Explicit validation is required for accurate 
        /// size calculation.
        /// </remarks>
        private static (Size, object) GetSizeOrDbNullAsObject(object item, PgConverter converter, Size writeRequirement)
        {
            object writeState = default;

            return (item is DBNull or null ? Constants.NULL_SIZE : Accessors.GetSizeOrDbNullAsObject(default, converter, DataFormat.Binary, writeRequirement, item, ref writeState).Value, writeState);
        }
    }
}