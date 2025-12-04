using Npgsql.Internal;

using System;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal readonly struct Value(object item, PgConverterResolution resolution, Size writeRequirement)
    {
        /// <summary>
        /// Size of the item headers.
        /// </summary
        /// 
        /// <remarks>
        /// OID + Length.
        /// </remarks>
        public const int METADATA_SIZE = sizeof(uint) + sizeof(int);

        /// <summary>
        /// 
        /// </summary>
        public object Item
        {
            get => item;
        }

        /// <summary>
        /// The data type's OID - a unique id identifying 
        /// the data type in a given database (in pg_type).
        /// </summary>
        public uint OID
        {
            get => resolution.PgTypeId.Oid.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public PgConverter Converter
        {
            get => resolution.Converter;
        }

        /// <summary>
        /// 
        /// </summary>
        public Size WriteRequirement
        {
            get => writeRequirement;
        }

        /// <summary>
        /// Size of the item.
        /// </summary
        public int Size
        {
            get;
            init => GetSize(item, resolution, writeRequirement);
        }

        /// <summary>
        /// 
        /// </summary>
        public object WriteState
        {
            get;
            init => field = GetSize(item, resolution, writeRequirement);
        }

        private int GetSize(object item, PgConverterResolution resolution, Size writeRequirement)
        {
            object writeState = default;

            return item is DBNull or null ? Constants.NULL_SIZE : Accessors.GetSizeOrDbNullAsObject(default, resolution.Converter, DataFormat.Binary, writeRequirement, item, ref writeState).Value.Value;
        }
    }
}