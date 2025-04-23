using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsPaged
{
    public class GetCatsQueryValidator : AbstractValidator<GetCatsQuery>
    {
        public GetCatsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100.");
        }
    }
}
