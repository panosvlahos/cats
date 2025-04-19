using Hangfire;
using MediatR;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class FetchCatsCommandHandler : IRequestHandler<FetchCatsCommand, string>
    {
        public Task<string> Handle(FetchCatsCommand request, CancellationToken cancellationToken)
        {
            // Enqueue Hangfire background job
            var jobId = BackgroundJob.Enqueue<FetchCatsJob>(job => job.ExecuteAsync());

            return Task.FromResult(jobId);
        }
    }
}
