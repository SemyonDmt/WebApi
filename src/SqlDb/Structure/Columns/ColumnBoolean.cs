namespace SqlDb.Structure.Columns
{
    public class ColumnBoolean : Column
    {
        public ColumnBoolean(string name, bool isNull) : base(name, isNull)
        {
        }

        public override string SqlRequestCreateColumn()
        {
            return $"{Name} bit {SqlCanBeNull()}";
        }
    }
}