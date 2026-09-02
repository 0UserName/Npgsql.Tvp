using Npgsql.Tvp.Tests.Parameters.Contracts;

using System.Data;

namespace Npgsql.Tvp.Tests.Parameters
{
    internal sealed class DataTableParameter(DataTable value) : IParameter<DataTable>
    {
        /// <inheritdoc/>
        public int Rows
        {
            get => value.Rows.Count;
        }

        /// <inheritdoc/>
        public DataTable Get()
        {
            return value;
        }
    }
}