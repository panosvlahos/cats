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

namespace Application.Queries.GetCatById
{
    public class GetCatByIdHandler : IRequestHandler<GetCatByIdQuery, CatDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCatByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CatDto?> Handle(GetCatByIdQuery request, CancellationToken cancellationToken)
        {

            var cat = await _unitOfWork.CatRepository.GetCatByIdAsync(request.CatId);

            if (cat == null)
                return null;


            var catDto = _mapper.Map<CatDto>(cat);

            return catDto;
        }
    }
}
