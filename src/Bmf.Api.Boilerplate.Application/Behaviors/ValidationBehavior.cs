using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Bmf.Api.Boilerplate.Application.Behaviors;

/// <summary>Runs FluentValidation validators before the handler.</summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (validators.Any())
        {
            ValidationContext<TRequest> context = new(request);
            List<FluentValidation.Results.ValidationFailure> failures = [];

            foreach (IValidator<TRequest> validator in validators)
            {
                FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(context, ct).ConfigureAwait(false);
                failures.AddRange(result.Errors.Where(e => e is not null));
            }

            if (failures.Count > 0)
            {
                string message = string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
                throw new ValidationException(message, failures);
            }
        }

        return await next().ConfigureAwait(false);
    }
}
