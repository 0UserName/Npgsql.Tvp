using Npgsql.Tvp.Tests.Parameters.Contracts;

using System.Data;

namespace Npgsql.Tvp.Tests.Parameters
{
    internal sealed class DataReaderParameter(IDataReader value) : IParameter<IDataReader>
    {
        /// <inheritdoc/>
        public int Rows
        {
            get => value.RecordsAffected;
        }

        /// <inheritdoc/>
        public IDataReader Get()
        {
            return value;
        }
    }
}