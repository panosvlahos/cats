using Application.Commands;
using Hangfire;
using Interfaces.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Cats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public JobsController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public IActionResult GetJobStatus(string id)
        {
            var jobData = Hangfire.JobStorage.Current.GetMonitoringApi().JobDetails(id);

            if (jobData == null)
                return NotFound(new { status = "Not Found", jobId = id });

            var history = jobData.History.FirstOrDefault();
            var state = history?.StateName ?? "Unknown";
            var createdAt = history?.CreatedAt.ToUniversalTime();

            return Ok(new
            {
                jobId = id,
                status = state,
                createdAt = createdAt
            });
        }
    }
}
