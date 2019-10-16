namespace SqlDb.Structure.Columns
{
    public class ColumnString : Column
    {
        private readonly int _size;

        public ColumnString(string name, bool isNull, int size) : base(name, isNull)
        {
            _size = size;
        }

        public override string SqlRequestCreateColumn()
        {
            return $"{Name} nvarchar({_size}) {SqlCanBeNull()}";
        }
    }
}