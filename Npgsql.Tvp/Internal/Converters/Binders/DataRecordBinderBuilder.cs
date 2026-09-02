using Npgsql.Tvp.Internal.Converters.Binders.Contracts.Abstracts;

using System.Data;

namespace Npgsql.Tvp.Internal.Converters.Binders
{
    internal sealed class DataRecordBinderBuilder<TRecord>() : AbstractDataRecordBinderBuilder<TRecord>() where TRecord : IDataRecord
    { }
}