using FluentValidation;
using SqlDb.Model.Structure.Columns;

namespace SqlDb.Validators
{
    public class ColumnArrayValidator: AbstractValidator<Column[]>
    {
        public ColumnArrayValidator()
        {
            RuleFor(columns => columns)
                .NotNull()
                .NotEmpty();

            RuleForEach(columns => columns)
                .SetValidator(new ColumnValidator());
        }
    }
}