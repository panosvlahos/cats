using FluentValidation;
using Contracts;
using Entities.Models;
using Interfaces.Interfaces;
using MediatR;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsPaged
{
    public class GetCatsHandler : IRequestHandler<GetCatsQuery, List<CatDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCatsQuery> _validator;

        public GetCatsHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<GetCatsQuery> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<CatDto>> Handle(GetCatsQuery request, CancellationToken cancellationToken)
        {
            // Validate the query using the injected validator
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If validation fails, throw a validation exception with the error details
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Proceed with fetching the cats if validation passes
            var cats = await _unitOfWork.CatRepository.GetCatsAsync(request.Page, request.PageSize);

            // Map the list of Cat entities to CatDto objects
            var catDtos = _mapper.Map<List<CatDto>>(cats);
            return catDtos;
        }
    }
}
