using Npgsql.Internal;
using Npgsql.Internal.Postgres;

using Npgsql.Tvp.Internal.Converters;
using Npgsql.Tvp.Internal.Converters.Models;

using Npgsql.Tvp.Internal.Resolvers.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Resolvers
{
    internal sealed class DRConverterResolver(PgSerializerOptions options) : AbstractConverterResolver<IDataReader>
    {
        private readonly
            DRConverter _converter = new
            DRConverter
            (options);

        /// <inheritdoc/>
        protected override PgTypeId GetArrayType(IDataReader value)
        {
            return value.GetArrayType(options);
        }

        /// <inheritdoc/>
        protected override PgConverter GetConverter()
        {
            return _converter;
        }
    }
}