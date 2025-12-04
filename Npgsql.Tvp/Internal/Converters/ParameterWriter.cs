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
                // Sum of the sizes of the
                // nearest writable values.
                if (writer.ShouldFlush(parameter.MetadataSize))
                {
                    await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                }


                writer.WriteInt32(Constants.DIMENSIONS);
                writer.WriteInt32(Constants.FLAGS);
                writer.WriteUInt32(parameter.OID);
                writer.WriteInt32(parameter.RowsCount);
                writer.WriteInt32(Constants.LOWER_BOUND);


                for (int i = 0; i < parameter.RowsCount; i++)
                {
                    // Sum of the sizes of the
                    // nearest writable values:
                    //
                    // 1. Row size
                    // 2. Columns count
                    if (writer.ShouldFlush(sizeof(int) + sizeof(int)))
                    {
                        await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                    }


                    writer.WriteInt32(parameter.GetRowSize(i));
                    writer.WriteInt32(parameter.ColumnsCount);


                    for (int j = 0; j < parameter.ColumnsCount; j++)
                    {
                        // Sum of the sizes of the
                        // nearest writable values:
                        //
                        // 1. Item OID
                        // 2. Item size
                        if (writer.ShouldFlush(sizeof(uint) + sizeof(int)))
                        {
                            await writer.FlushAsync(cancellationToken).ConfigureAwait(default);
                        }


                        Value value = parameter[i, j];

                        writer.WriteUInt32(value.OID);
                        writer.WriteInt32(value.Size);


                        if (value.Size != Constants.NULL_SIZE)
                        {
                            using (await writer.BeginNestedWriteAsync(value.WriteRequirement, value.Size, value.WriteState, cancellationToken).ConfigureAwait(default))
                            {
                                await Accessors.WriteAsObjectAsync(value.Converter, writer, value.Item, cancellationToken).ConfigureAwait(default);
                            }
                        }
                    }
                }
            }
        }
    }
}