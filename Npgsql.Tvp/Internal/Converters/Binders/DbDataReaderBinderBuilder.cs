using Npgsql.Tvp.Internal.Converters.Binders.Contracts.Abstracts;

using System;
using System.Data.Common;

using System.Linq.Expressions;

namespace Npgsql.Tvp.Internal.Converters.Binders
{
    internal sealed class DbDataReaderBinderBuilder() : AbstractDataRecordBinderBuilder<DbDataReader>(nameof(DbDataReader.GetFieldValue))
    {
        /// <inheritdoc/>
        protected override Expression CreateDefaultGetterExpression(ParameterExpression record, ConstantExpression ordinal, Type type)
        {
            return CreateTypedGetterExpression(record, DefaultGetter.MakeGenericMethod(type), ordinal);
        }
    }
}