﻿using Interfaces.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class FetchCatsCommand : IRequest<string> {
        public int Count { get; set; }
    }
}
