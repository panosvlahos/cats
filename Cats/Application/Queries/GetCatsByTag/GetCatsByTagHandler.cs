using AutoMapper;
using Contracts;
using Entities.Models;
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
        public GetCatsByTagHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CatDto>> Handle(GetCatsByTagQuery request, CancellationToken cancellationToken)
        {
            var cat = await _unitOfWork.CatRepository.GetCatsByTagAsync(request.Tag);

            if (cat == null)
                return null;

           
            var catDto = _mapper.Map<List<CatDto>>(cat);

            return catDto;
        }
    }
}
