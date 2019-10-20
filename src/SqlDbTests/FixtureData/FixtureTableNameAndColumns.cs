using System.Linq;
using AutoFixture;

namespace SqlDbTests.FixtureData
{
    public static class FixtureTableNameAndColumns
    {
        private static readonly Fixture Fixture = new Fixture();
        
        public static string BuildTableName()
        {
            return "Orders";
        }

        public static string BuildColumns()
        {
            return Fixture.Create<string>();
        }
    }
}