using FluentValidation;
using Hangfire;
using MediatR;
using Services.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class FetchCatsCommandHandler : IRequestHandler<FetchCatsCommand, string>
    {
        private readonly IValidator<FetchCatsCommand> _validator;

        public FetchCatsCommandHandler(IValidator<FetchCatsCommand> validator)
        {
            _validator = validator;
        }

        public async Task<string> Handle(FetchCatsCommand request, CancellationToken cancellationToken)
        {
            // Validate the command using the injected validator
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If validation fails, throw a validation exception with the error details
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Proceed with your logic (like enqueuing the Hangfire job)
            var jobId = BackgroundJob.Enqueue<FetchCatsJob>(job => job.ExecuteAsync());

            return jobId;
        }
    }
}
