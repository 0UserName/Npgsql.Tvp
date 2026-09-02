namespace Npgsql.Tvp.Tests.Parameters.Contracts
{
    public interface IParameter<T>
    {
        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        int Rows
        {
            get;
        }

        T Get();
    }
}