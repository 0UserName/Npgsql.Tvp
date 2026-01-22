using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDR(IDataReader value, uint oid, int rowsCount, PgSerializerOptions options) : AbstractParameter(value.FieldCount, rowsCount, oid, options)
    {
        /// <inheritdoc/>
        protected override void FillBuffer()
        {
            while (value.Read())
            {
                for (int i = 0; i < value.FieldCount; i++)
                {
                    Buffer.Write(CreateValue(value.GetValue(i), value.GetFieldType(i)));
                }
            }
        }

        public ParameterDR(IDataReader value, uint oid, PgSerializerOptions options) : this(value, oid, 32, options)
        { }
    }
}