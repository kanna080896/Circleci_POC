using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Concrete;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;
using Microsoft.SqlServer.Server;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.SqLiteCache.Helpers;

public class SqliteHelperTests
{
    private readonly ISqLiteHelper _helper;

    public SqliteHelperTests()
    {   
        _helper = new SqLiteHelper();
    }

    [Fact]
    public void GetSqlToDropTableIfExists_Success()
    {
        var result = _helper.GetSqlToDropTableIfExists("test");
        const string expected = "DROP TABLE IF EXISTS [test];";

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSqlToCreateTableIfNotExists_Success()
    {
        var schema = new DataTable();
        schema.Columns.Add("ColumnName");
        schema.Columns.Add("DataTypeName");
        schema.Columns.Add("DataType");
        schema.Columns.Add("ColumnSize");
        schema.Columns.Add("AllowDBNull");

        schema.Rows.Add("TestColumn", "INT", "INT", 1, false);

        var result = _helper.GetSqlToCreateTableIfNotExists(schema, "TestTable", new[] { "TestColumn" });
        const string expected = "create table if not exists [TestTable] ([TestColumn] INT not null, primary key([TestColumn]));";

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSqlToCreateTableIfNotExistsIndexCoumnZero_Success()
    {
        var schema = new DataTable();
        var result = _helper.GetSqlToCreateTableIfNotExists(schema, "TestTable", new string[] { });
        const string expected = "create table if not exists [TestTable] ();";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSqlToCreateIndexes_Success()
    {
        var result = _helper.GetSqlToCreateIndexes("TestTable", new[] { "TestColumn1", "TestColumn2" });
        const string expected = "create unique index [Idx_TestTable_TestColumn1] on [TestTable]([TestColumn1]);\r\nCREATE INDEX [Idx_TestTable_TestColumn2] ON [TestTable]([TestColumn2]);\r\n";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSqlToCreateIndexesIndexCoumnZero_Success()
    {
        var result = _helper.GetSqlToCreateIndexes("TestTable", new string[] { });
        const string expected = "";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetSqlToInsertRow_Success()
    {
        var result1 = _helper.GetSqlToInsertRow(1, "TestTable");
        const string expected1 = "INSERT INTO [TestTable] VALUES (@Parameter0)";
        Assert.Equal(expected1, result1);
    }

    [Fact]
    public void GetObjectArrayFromRow_Success()
    {
        var record = new SqlDataRecord(new SqlMetaData("Test", SqlDbType.Int));

        var result = _helper.GetObjectArrayFromRow(record);
        var expected = new List<object> {DBNull.Value};

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Quote_Success()
    {
        var result = _helper.GetSqlToDropTableIfExists("TestTable").Quote();
        const string expected = "'DROP TABLE IF EXISTS [TestTable];'";
        
        Assert.Equal(expected, result);
    }
}