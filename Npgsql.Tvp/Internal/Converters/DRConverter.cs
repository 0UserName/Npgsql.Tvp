using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Abstracts;

using Npgsql.Tvp.Internal.Converters.Models;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters
{
    internal sealed class DRConverter(PgSerializerOptions options) : AbstractConverter<IDataReader>
    {
        /// <inheritdoc/>
        protected override IParameter GetParameter(IDataReader value)
        {
            return new ParameterDR(value, value.GetArrayType(options).Oid.Value, options);
        }
    }
}