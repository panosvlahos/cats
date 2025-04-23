using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsByTag
{
    public class GetCatsByTagQueryValidator : AbstractValidator<GetCatsByTagQuery>
    {
        public GetCatsByTagQueryValidator()
        {
            RuleFor(x => x.Tag)
                .NotEmpty().WithMessage("Tag is required.")
                .MaximumLength(50).WithMessage("Tag must be 50 characters or fewer.");
        }
    }
}
