namespace SqlDb.Model.Structure.Columns
{
    public class ColumnId : Column
    {
        public ColumnId(string name) : base(name, false)
        {
        }

        public override string SqlRequestCreateColumn()
        {
            return $"{Name} int IDENTITY PRIMARY KEY";
        }

        public override string SqlRequestInsertColumn()
        {
            return string.Empty;
        }
    }
}