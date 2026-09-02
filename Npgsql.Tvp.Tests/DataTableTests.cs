using Npgsql.Tvp.Tests.Abstracts;

using Npgsql.Tvp.Tests.Parameters;

using System.Data;

using System.Threading.Tasks;

namespace Npgsql.Tvp.Tests
{
    public sealed class DataTableTests : AbstractTests<DataTable>
    {
        [TestCase(Data.Procedures.VARIABLE, Data.Types.VARIABLE, TestName = $"Call { Data.Procedures.VARIABLE } with { Data.Types.VARIABLE }")]
        [TestCase(Data.Procedures.CONSTANT, Data.Types.CONSTANT, TestName = $"Call { Data.Procedures.CONSTANT } with { Data.Types.CONSTANT }")]
        public async Task TestInsertAsync(string procedure, string type)
        {
            await TestInsertAsync(procedure, new DataTableParameterFactory().Create(type));
        }
    }
}