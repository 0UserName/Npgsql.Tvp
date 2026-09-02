namespace Npgsql.Tvp.Internal.Converters.Parameters.Contracts
{
    internal interface IParameterBinder
    {
        /// <summary>
        /// Binds null to the column
        /// at the specified ordinal.
        /// </summary>
        void Bind(int ordinal);

        /// <summary>
        /// Binds the
        /// specified non-null value to the
        /// column at the specified ordinal.
        /// </summary>
        void Bind<T>(int ordinal, T value);
    }
}