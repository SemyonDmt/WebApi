namespace SqlDb.Model
{
    public enum ColumnType : byte
    {
        none = 0,
        @int = 1,
        nvarchar = 2,
        datetime = 3,
        bit = 4
    }
}