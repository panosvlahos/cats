using Contracts;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatById
{
    public record GetCatByIdQuery(string CatId) : IRequest<CatDto?>;
}
