using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using NUnit.Framework;
using SqlDb.Commands.Tables;
using SqlDbTests.Environment;

namespace SqlDbTests.Commands.Tables
{
    public class CommandSelectTablesTests
    {
        [Test]
        public async Task ExecuteAsync_HappyPath_DataReturnGeneratedResponse()
        {
            var expectedResponse = new Fixture().CreateMany<string>().ToArray();
            var connection = new Mock<DbConnection>();

            connection.As<IDbConnection>()
                .SetupDapper(c => c.Query<string>(
                    It.IsAny<string>(),
                    null,
                    null,
                    true,
                    null,
                    null))
                .Returns(expectedResponse);

            var dbCommand = new FakeDbCommand {ConfigureDbConnection = connection.Object};

            var command = new CommandSelectTables();

            await command.ExecuteAsync(dbCommand);

            command.Data.Should().BeEquivalentTo(expectedResponse);
        }
    }
}