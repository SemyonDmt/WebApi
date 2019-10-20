using FluentValidation;

namespace SqlDb.Validators
{
    public class SelectSqlValidator : AbstractValidator<string>
    {
        public SelectSqlValidator()
        {
            RuleFor(tableName => tableName)
                .Must(str => !str.CheckForSqlInjection()).WithMessage("Not must contains sql words")
                .Matches("^[a-zA-Z,]+$").WithMessage("Only letters and comma");
        }
    }
}