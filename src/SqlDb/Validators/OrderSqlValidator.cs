using FluentValidation;

namespace SqlDb.Validators
{
    public class OrderSqlValidator: AbstractValidator<string>
    {
        public OrderSqlValidator()
        {
            RuleFor(tableName => tableName)
                .Must(str => !str.CheckForSqlInjection()).WithMessage("Not must contains sql words")
                .Matches("^[a-zA-Z, ]+$").WithMessage("Only letters and comma");
        }
    }
}