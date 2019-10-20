using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SqlDb.Commands.Rows;
using SqlDbTests.Environment;
using SqlDbTests.FixtureData;

namespace SqlDbTests.Commands.Rows
{
    public class CommandInsertRowTests
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
        public void InsertCommand_Default_IsSuccessfulFalseAndDataIsNull()
        {
            var cmd = new CommandInsertRow(_tableNameFixture, _colsFixture);

            cmd.IsSuccessful.Should().BeFalse();
            cmd.Data.Should().BeNull();
        }

        [Test]
        public async Task ExecuteAsync_HappyPath_DataReturnGeneratedResponse()
        {
            var expectedResponse = new Fixture().Create<string>();
            var dbCommand = CreateDbCommandForHappyPath(expectedResponse: expectedResponse);

            var command = new CommandInsertRow(_tableNameFixture, _colsFixture);

            await command.ExecuteAsync(dbCommand);

            command.Data.Should().Be(expectedResponse);
        }

        [Test]
        public async Task ExecuteAsync_HappyPath_CreateIdReturnGeneratedId()
        {
            var expectedId = new Fixture().Create<int>();
            var dbCommand = CreateDbCommandForHappyPath(expectedId: expectedId);

            var command = new CommandInsertRow(_tableNameFixture, _colsFixture);

            await command.ExecuteAsync(dbCommand);

            command.CreatedId.Should().Be(expectedId);
        }

        [Test]
        public async Task ExecuteAsync_HappyPath_IsSuccessfulReturnTrue()
        {
            var dbCommand = CreateDbCommandForHappyPath();

            var command = new CommandInsertRow(_tableNameFixture, _colsFixture);

            await command.ExecuteAsync(dbCommand);

            command.IsSuccessful.Should().BeTrue();
        }

        [Test]
        public async Task ExecuteAsync_VerifyBehavior()
        {
            var dbCommand = CreateDbCommandForHappyPath();
            var command = new CommandInsertRow(_tableNameFixture, _colsFixture);

            await command.ExecuteAsync(dbCommand);

            dbCommand.CountCallCommandText.Should().Be(1);
            dbCommand.CountCallDbParameterCollection.Should().Be(1);
            dbCommand.CountCallExecuteScalarAsync.Should().Be(1);
        }

        //Todo добавить проверку на Exceptions

        private FakeDbCommand CreateDbCommandForHappyPath(string expectedResponse = null, int? expectedId = int.MinValue)
        {
            expectedResponse ??= new Fixture().Create<string>();

            if (expectedId == int.MinValue)
                expectedId = new Fixture().Create<int>();

            var dbCommand = new FakeDbCommand();

            dbCommand.ConfigureExecuteScalarAsync = expectedResponse;
            dbCommand.SetOutputParameterValueAfterExecute($"@Id", expectedId);

            return dbCommand;
        }
    }
}