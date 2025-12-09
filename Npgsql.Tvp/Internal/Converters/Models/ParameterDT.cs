using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Models
{
    internal sealed class ParameterDT(DataTable parameter, PgSerializerOptions options) : AbstractParameter(parameter.TableName, parameter.Columns.Count, parameter.Rows.Count, options)
    {
        /// <inheritdoc/>
        protected override void FillBuffer()
        {
            int rIndex, cIndex;

            for (int i = 0; i < Buffer.Values.Count; i++)
            {
                rIndex = i / ColumnsCount;
                cIndex = i % ColumnsCount;

                Buffer.Write(CreateValue(parameter.Rows[rIndex][cIndex], parameter.Columns[cIndex].DataType));
            }
        }
    }
}