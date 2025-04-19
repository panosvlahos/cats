using Contracts;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsPaged
{
    public record GetCatsQuery(int Page, int PageSize) : IRequest<List<CatDto>>;
}
