![Release workflow](https://github.com/0UserName/npgsql.tvp/actions/workflows/release.yml/badge.svg)



# Motivation

The plugin was created to emulate table-valued parameters ([supported](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/sql/table-valued-parameters) in SQL Server but [not](https://github.com/npgsql/npgsql/issues/5415) PostgreSQL) to enhance cross-database compatibility.



# Usage

Add a dependency on this package and create a `NpgsqlDataSource`. Once this is done, you can use `DataTable` and `DbDataReader` types when interacting with PostgreSQL:



```csharp
NpgsqlDataSourceBuilder builder = new
NpgsqlDataSourceBuilder
("Host=localhost;Username=postgres;Password=root;Database=postgres");

builder.UseTvp();

// Connect..

DataTable dt = new
DataTable
("schema.compositeType");

dt.Columns.Add(new DataColumn("Column1"));
dt.Columns.Add(new DataColumn("Column2"));
dt.Columns.Add(new DataColumn("Column3"));

dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");
dt.Rows.Add("Column1_value", "Column2_value", "Column3_value");

NpgsqlCommand cmd = new
NpgsqlCommand
("schema.storedProcedure", connection);

cmd.Parameters.Add(new NpgsqlParameter(nameof(dt), dt));

// Set command type and execute..
```



The plugin processes the parameter as an array of a composite type that was previously created on the server:



```sql
CREATE PROCEDURE schema.storedProcedure(IN dt schema.compositeType[])
```



> [!NOTE]
> Specifying types using [NpgsqlParameter.DataTypeName](https://www.npgsql.org/doc/api/Npgsql.NpgsqlParameter.html#Npgsql_NpgsqlParameter_DataTypeName) is not supported.



## DataTable

Use [TableName](https://learn.microsoft.com/en-us/dotnet/api/system.data.datatable.tablename?view=net-10.0) property.



## DbDataReader

Use [GetSchemaTable](https://learn.microsoft.com/en-us/dotnet/api/system.data.idatareader.getschematable?view=net-10.0) method. The table must include a column named [BaseTableName](https://learn.microsoft.com/en-us/dotnet/api/system.data.common.schematablecolumn.basetablename?view=net-10.0).