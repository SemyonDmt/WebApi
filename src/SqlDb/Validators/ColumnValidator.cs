using FluentValidation;
using SqlDb.Model.Structure.Columns;

namespace SqlDb.Validators
{
    public class ColumnValidator: AbstractValidator<Column>
    {
        public ColumnValidator()
        {
            RuleFor(col => col.Name)
                .NotNull()
                .NotEmpty()
                .Must(str => !str.CheckForSqlInjection()).WithMessage("Not must contains sql words")
                .Matches("^[a-zA-Z]+$").WithMessage("Only letters");
        }
    }
}