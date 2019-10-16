using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SqlDb.Requests.Rows;
using SqlDbTests.FixtureData;

namespace SqlDbTests.Requests.Rows
{
    public class RequestInsertTests
    {
        private (string colName, object value)[] _colsFixture;
        private string _tableNameFixture;

        [SetUp]
        public void Setup()
        {
            _colsFixture = FixtureTableNameAndColumns.BuildColumns();
            _tableNameFixture = FixtureTableNameAndColumns.BuildTableName();
        }

        [TestCase("TestTable", "INSERT INTO TestTable ")]
        [TestCase("Users", "INSERT INTO Users ")]
        public void ReturnSqlInsert_SetValidTableName_StartWithExpected(string tableName, string expected)
        {
            var request = new RequestInsert(tableName, _colsFixture);

            var result = request.Sql();

            result.Should().StartWith(expected);
        }

        [TestCase("TestTable", "SELECT * FROM TestTable ")]
        [TestCase("Users", "SELECT * FROM Users ")]
        [TestCase("TestTable", " WHERE TestTableId=@TestTableId ")]
        [TestCase("Users", " WHERE UsersId=@UsersId ")]
        public void ReturnSqlInsert_SetValidTableName_ContainExpected(string tableName, string expected)
        {
            var request = new RequestInsert(tableName, _colsFixture);

            var result = request.Sql();

            result.Should().Contain(expected);
        }

        [TestCase(" (col1) VALUES", "col1")]
        [TestCase(" (col1, col2, col3, col4) VALUES", "col1", "col2", "col3", "col4")]
        [TestCase(" VALUES (@col1)", "col1")]
        [TestCase(" VALUES (@col1, @col2, @col3, @col4)", "col1", "col2", "col3", "col4")]
        public void ReturnSqlInsert_SetValidCols_ContainExpected(string expected, params string[] cols)
        {
            var request = new RequestInsert(_tableNameFixture, GenerateCols(cols));

            var result = request.Sql();

            result.Should().Contain(expected);
        }

        [Test]
        public void ReturnSqlInsert_SetValidTableNameAndCols_ReturnValidSqlRequest()
        {
            var tableName = "TestTable";
            var cols = GenerateCols(new[] {"col1", "col2", "col3", "col4"});

            var request = new RequestInsert(tableName, cols);

            var result = request.Sql();

            var expected =
                "INSERT INTO TestTable (col1, col2, col3, col4) VALUES (@col1, @col2, @col3, @col4); SET @TestTableId=SCOPE_IDENTITY(); SELECT * FROM TestTable WHERE TestTableId=@TestTableId FOR JSON PATH";
            result.Should().Contain(expected);
        }

        [TestCase("")]
        [TestCase(null)]
        public void ReturnSqlInsert_SetNotValidTableName_ReturnArgumentNullExceptions(string tableName)
        {
            Assert.Throws<ArgumentNullException>(() => new RequestInsert(tableName, _colsFixture));
        }

        [Test]
        public void ReturnSqlInsert_SetColsIsNull_ReturnArgumentNullExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new RequestInsert(_tableNameFixture, ((string colName, object value)[]) null));
        }

        [TestCase(null)]
        [TestCase(null, "col1")]
        [TestCase("col1", null)]
        [TestCase("")]
        [TestCase("col1", "")]
        [TestCase("", "col1")]
        public void ReturnSqlInsert_SetColsIsNull_ReturnArgumentNullExceptions(params string[] cols)
        {
            Assert.Throws<ArgumentNullException>(() => new RequestInsert(_tableNameFixture, GenerateCols(cols)));
        }

        [Test]
        public void ReturnParametersInsert_SetValidAllParameters_VerifyParametersInsertContainsId()
        {
            var expected = new SqlParameter
            {
                ParameterName = $"@{_tableNameFixture}Id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var request = new RequestInsert(_tableNameFixture, _colsFixture);

            var result = request.Parameters();

            var id = result.SingleOrDefault(s => s.ParameterName == expected.ParameterName);
            id.Should().NotBeNull();
            id.SqlDbType.Should().Be(expected.SqlDbType);
            id.Direction.Should().Be(expected.Direction);
        }

        [Test]
        public void GetParameterId_SetValidAllParameters_ReturnParameterId()
        {
            var expected = new SqlParameter
            {
                ParameterName = $"@{_tableNameFixture}Id",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            var request = new RequestInsert(_tableNameFixture, _colsFixture);

            var result = request.GetParameterId();

            result.ParameterName.Should().Be(expected.ParameterName);
            result.SqlDbType.Should().Be(expected.SqlDbType);
            result.Direction.Should().Be(expected.Direction);
        }

        [Test]
        public void ReturnParametersInsert_SetValidAllParameters_VerifyAllParameters()
        {
            var request = new RequestInsert(_tableNameFixture, _colsFixture);

            var result = request.Parameters();

            VerifyAllParameters(result, _colsFixture);
        }

        private static (string colName, object value)[] GenerateCols(string[] colsName)
        {
            var fixture = new Fixture();
            return colsName.Select(c => (c, fixture.Create<object>()))
                .ToArray();
        }

        private static void VerifyAllParameters(SqlParameter[] sqlParameters, (string colName, object value)[] cols)
        {
            foreach (var col in cols)
            {
                var foundParameter = sqlParameters.Single(s => s.ParameterName == col.colName);
                VerifyParameter(foundParameter, col);
            }
        }

        private static void VerifyParameter (SqlParameter sqlParam, (string colName, object value) colVal)
        {
            sqlParam.Should().NotBeNull();
            sqlParam.ParameterName.Should().Be(colVal.colName);
            sqlParam.Direction.Should().Be(ParameterDirection.Input);
            sqlParam.Value.Should().Be(colVal.value);
        }
    }
}