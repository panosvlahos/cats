using FluentValidation;
namespace Application.Commands
{
    public class FetchCatsCommandValidator : AbstractValidator<FetchCatsCommand>
    {
        public FetchCatsCommandValidator()
        {
            RuleFor(x => x.Count)
             .GreaterThan(0).WithMessage("You must fetch at least 1 cat.")
             .LessThanOrEqualTo(50).WithMessage("You can fetch a maximum of 50 cats.");
        }
    }
}
