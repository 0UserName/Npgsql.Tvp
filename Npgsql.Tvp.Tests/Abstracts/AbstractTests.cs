using Dapper;

using Npgsql.Tvp.Tests.Parameters.Contracts;

using System;
using System.Data;

using System.Threading.Tasks;

using Testcontainers.PostgreSql;

namespace Npgsql.Tvp.Tests.Abstracts
{
    public abstract class AbstractTests<TParameter> : IAsyncDisposable
    {
        private PostgreSqlContainer _container;

        private NpgsqlDataSource _source;

        /// <summary>
        /// Performs a one‑time configuration before running tests.
        /// </summary>
        /// 
        /// <remarks>
        /// Starts the dependent database and copies the SQL scripts to the container
        /// before it starts. The PostgreSQL container runs the scripts automatically
        /// during startup, creating the database schema.
        /// </remarks>
        [OneTimeSetUp]
        protected async Task SetupAsync()
        {
            _container = new PostgreSqlBuilder(Environment.GetEnvironmentVariable("NPGSQL_TVP_PG_IMAGE") ?? "postgres:14-alpine").WithResourceMapping("Init/", "/docker-entrypoint-initdb.d/").Build();

            await _container.StartAsync();

            _source = new NpgsqlDataSourceBuilder(_container.GetConnectionString()).UseTvp().Build();
        }

        /// <returns>
        /// The number of
        /// rows affected.
        /// </returns>
        protected async Task<long> ExecuteAsync(string procedure, TParameter parameter)
        {
            DynamicParameters param = new
            DynamicParameters
            ();

            param.Add("@i_p", direction: (ParameterDirection)1, value: parameter, dbType: DbType.Object);
            param.Add("@o_p", direction: (ParameterDirection)2);

            await _source.CreateConnection().ExecuteAsync(procedure, param, commandType: CommandType.StoredProcedure);

            return param.Get<long>("@o_p");
        }

        /// <summary>
        /// Executes a stored procedure and asserts
        /// that the number of affected rows equals
        /// the row count of the parameter.
        /// </summary>
        /// 
        /// <param name="procedure">
        /// Name of the stored procedure.
        /// </param>
        protected Task TestInsertAsync(string procedure, IParameter<TParameter> parameter)
        {
            return Assert.ThatAsync(() => ExecuteAsync(procedure, parameter.Get()), Is.EqualTo(parameter.Rows));
        }

        /// <inheritdoc/>
        [OneTimeTearDown]
        public ValueTask DisposeAsync()
        {
            return _container.DisposeAsync();
        }
    }
}