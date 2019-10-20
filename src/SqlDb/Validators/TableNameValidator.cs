using FluentValidation;

namespace SqlDb.Validators
{
    public class TableNameValidator : AbstractValidator<string>
    {
        public TableNameValidator()
        {
            RuleFor(tableName => tableName)
                .NotNull()
                .NotEmpty()
                .Must(str => !str.CheckForSqlInjection()).WithMessage("Not must contains sql words")
                .Matches("^[a-zA-Z]+$").WithMessage("Table name Only letters");
        }
    }
}