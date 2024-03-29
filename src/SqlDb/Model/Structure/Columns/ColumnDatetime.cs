namespace SqlDb.Model.Structure.Columns
{
    public class ColumnDatetime : Column
    {
        public ColumnDatetime(string name, bool isNull) : base(name, isNull)
        {
        }

        public override string SqlRequestCreateColumn()
        {
            return $"{Name} datetime {SqlCanBeNull()}";
        }

        public override string SqlRequestInsertColumn()
        {
            return $"{Name} datetime";
        }
    }
}