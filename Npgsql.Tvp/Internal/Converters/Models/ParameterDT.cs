using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDT(DataTable value, PgSerializerOptions options) : AbstractParameter(value.TableName, value.Columns.Count, value.Rows.Count, options)
    {
        /// <inheritdoc/>
        protected override void FillBuffer()
        {
            for (int i = 0; i < ColumnsCount * value.Rows.Count; i++)
            {
                int rIndex = i / ColumnsCount;
                int cIndex = i % ColumnsCount;

                Buffer.Write(CreateValue(value.Rows[rIndex][cIndex], value.Columns[cIndex].DataType));
            }
        }
    }
}