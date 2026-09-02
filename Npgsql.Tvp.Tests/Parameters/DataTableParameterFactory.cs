using Npgsql.Tvp.Tests.Parameters.Contracts;
using Npgsql.Tvp.Tests.Parameters.Contracts.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Tests.Parameters
{
    internal sealed class DataTableParameterFactory : AbstractParameterFactory<IParameter<DataTable>>
    {
        /// <inheritdoc/>
        public override IParameter<DataTable> Create(string type)
        {
            return new DataTableParameter(ReadToDataTable(type));
        }
    }
}