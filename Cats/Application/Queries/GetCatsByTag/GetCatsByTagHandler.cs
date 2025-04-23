using AutoMapper;
using Contracts;
using Entities.Models;
using FluentValidation;
using Interfaces.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsByTag
{
    public class GetCatsByTagHandler : IRequestHandler<GetCatsByTagQuery, List<CatDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<GetCatsByTagQuery> _validator;
        public GetCatsByTagHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<GetCatsByTagQuery> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<List<CatDto>> Handle(GetCatsByTagQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var cat = await _unitOfWork.CatRepository.GetCatsByTagAsync(request.Tag);

            if (cat == null)
                return null;

           
            var catDto = _mapper.Map<List<CatDto>>(cat);

            return catDto;
        }
    }
}
