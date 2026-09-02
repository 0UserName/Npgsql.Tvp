using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Parameters.Contracts;

using System;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters.Contracts.Abstracts
{
    internal abstract class AbstractConverter<TParameter> : PgStreamingConverter<TParameter>
    {
        protected abstract IParameter CreateParameter(TParameter value);

        /// <inheritdoc/>
        public override TParameter Read(PgReader reader)
        {
            throw new NotSupportedException($"{ typeof(TParameter).FullName } is not supported");
        }

        /// <inheritdoc/>
        public override ValueTask<TParameter> ReadAsync(PgReader reader, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"{ typeof(TParameter).FullName } is not supported");
        }

        /// <inheritdoc/>
        public override Size GetSize(SizeContext context, TParameter value, ref object writeState)
        {
            IParameter parameter = CreateParameter(value);

            writeState = parameter;

            return parameter.CalculateSize();
        }

        /// <inheritdoc/>
        public override void Write(PgWriter writer, TParameter value)
        {
            WriteAsync(writer, value).AsTask().Wait();
        }

        /// <inheritdoc/>
        public override ValueTask WriteAsync(PgWriter writer, TParameter value, CancellationToken cancellationToken = default)
        {
            return ParameterWriter.WriteAsync(writer, cancellationToken);
        }
    }
}