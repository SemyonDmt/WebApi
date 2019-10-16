using System;

namespace SqlDb.Structure.Columns
{
    public abstract class Column
    {
        protected readonly string Name;
        private readonly bool _isNull;

        protected Column(string name, bool isNull)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _isNull = isNull;
        }

        protected string SqlCanBeNull() => _isNull ? "NULL" : "NOT NULL";
        public abstract string SqlRequestCreateColumn();
    }
}