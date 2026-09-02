namespace Npgsql.Tvp.Tests
{
    internal static class Data
    {
        public static class Types
        {
            public const string VARIABLE = "dbo.table_valued_variable.xml";
            public const string CONSTANT = "dbo.table_valued_constant.xml";
        }

        public static class Procedures
        {
            public const string VARIABLE = "dbo.table_valued_variable_insert";
            public const string CONSTANT = "dbo.table_valued_constant_insert";
        }
    }
}