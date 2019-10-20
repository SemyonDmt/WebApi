using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using AutoFixture;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using NUnit.Framework;
using SqlDb.Commands.Tables;
using SqlDbTests.Environment;
using SqlDbTests.FixtureData;

namespace SqlDbTests.Commands.Tables
{
    public class CommandSelectTablesTests
    {
        private string _colsFixture;
        private string _tableNameFixture;

        [SetUp]
        public void Setup()
        {
            _colsFixture = FixtureTableNameAndColumns.BuildColumns();
            _tableNameFixture = FixtureTableNameAndColumns.BuildTableName();
        }

        [Test]
        public async Task ExecuteAsync_HappyPath_DataReturnGeneratedResponse()
        {
            var expectedResponse = new Fixture().Create<string>();
            var dbCommand = CreateDbCommandForHappyPath(expectedResponse: expectedResponse);

            var command = new CommandSelectTables();

            await command.ExecuteAsync(dbCommand);

            command.Data.Should().Be(expectedResponse);
        }

        private FakeDbCommand CreateDbCommandForHappyPath(string expectedResponse)
        {
            expectedResponse ??= new Fixture().Create<string>();

            var dbCommand = new FakeDbCommand();

            dbCommand.ConfigureExecuteScalarAsync = expectedResponse;

            return dbCommand;
        }
    }
}