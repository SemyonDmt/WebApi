using System.Linq;
using AutoFixture;

namespace SqlDbTests.FixtureData
{
    public static class FixtureTableNameAndColumns
    {
        private static readonly Fixture Fixture = new Fixture();
        
        public static string BuildTableName()
        {
            return Fixture.Create<string>();
        }

        public static (string colName, object value)[] BuildColumns()
        {
            return Fixture.CreateMany<(string colName, object value)>().ToArray();
        }
    }
}