namespace SqlDb.Model
{
    public class Column
    {
        public bool IsKey { get; set; }
        public string Name { get; set; }
        public ColumnType ColumnType { get; set; }
        public bool IsNull { get; set; }
        public int? Size { get; set; }
    }
}