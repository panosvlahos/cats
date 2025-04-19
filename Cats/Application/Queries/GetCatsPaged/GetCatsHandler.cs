using Contracts;
using Entities.Models;
using Interfaces.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mappings;
namespace Application.Queries.GetCatsPaged
{
    public class GetCatsHandler : IRequestHandler<GetCatsQuery, List<CatDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCatsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CatDto>> Handle(GetCatsQuery request, CancellationToken cancellationToken)
        {
            var cats = await _unitOfWork.CatRepository.GetCatsAsync(request.Page, request.PageSize);

            // Map the list of Cat entities to CatDto objects
            var catDtos = _mapper.Map<List<CatDto>>(cats);
            return catDtos;
        }
    }
}
