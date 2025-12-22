using Npgsql.Internal;

using Npgsql.Tvp.Internal.Converters.Models;
using Npgsql.Tvp.Internal.Converters.Models.Contracts;

using System.Threading;
using System.Threading.Tasks;

namespace Npgsql.Tvp.Internal.Converters
{
    internal static class ParameterWriter
    {
        public static async ValueTask WriteAsync(PgWriter writer, CancellationToken cancellationToken)
        {
            using (IParameter parameter = (IParameter)writer.Current.WriteState)
            {
                if (writer.ShouldFlush(parameter.MetadataSize))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                }

                writer.WriteInt32(Constants.DIMENSIONS);
                writer.WriteInt32(Constants.FLAGS);
                writer.WriteUInt32(parameter.OID);
                writer.WriteInt32 (parameter.RowsCount);
                writer.WriteInt32 (Constants.LOWER_BOUND);

                for (int i = 0; i < parameter.RowsCount; i++)
                {
                    if (writer.ShouldFlush(sizeof(int) + sizeof(int)))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }

                    writer.WriteInt32(parameter.GetRowSize(i));
                    writer.WriteInt32(parameter.ColumnsCount);

                    for (int j = 0; j < parameter.ColumnsCount; j++)
                    {
                        // Value OID + value size
                        if (writer.ShouldFlush(sizeof(uint) + sizeof(int)))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                        }

                        Value value = parameter[i, j];

                        writer.WriteUInt32(value.OID);

                        int size = value.BufferRequirement.Value;

                        writer.WriteInt32(size);

                        if (size != Constants.NULL_SIZE)
                        {
                            using (await writer.BeginNestedWriteAsync(value.BufferRequirement, size, value.WriteState, cancellationToken).ConfigureAwait(default))
                            {
                                await Accessors.WriteAsObjectAsync(value.Converter, writer, value.UnderlyingValue, cancellationToken).ConfigureAwait(default);
                            }
                        }
                    }
                }
            }
        }
    }
}