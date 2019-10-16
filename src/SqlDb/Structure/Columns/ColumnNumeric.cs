namespace SqlDb.Structure.Columns
{
    public class ColumnNumeric : Column
    {
        public ColumnNumeric(string name, bool isNull) : base(name, isNull)
        {
        }

        public override string SqlRequestCreateColumn()
        {
            return $"{Name} int {SqlCanBeNull()}";
        }
    }
}