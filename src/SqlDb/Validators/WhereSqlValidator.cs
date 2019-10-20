using FluentValidation;

namespace SqlDb.Validators
{
    public class WhereSqlValidator: AbstractValidator<string>
    {
        public WhereSqlValidator()
        {
            RuleFor(tableName => tableName)
                .Must(str => !str.CheckForSqlInjection()).WithMessage("Not must contains sql words");
        }
    }
}