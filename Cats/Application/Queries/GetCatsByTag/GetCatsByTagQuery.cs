using Contracts;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetCatsByTag
{
    public record GetCatsByTagQuery(string? Tag) : IRequest<List<CatDto>>;
}
