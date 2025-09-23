using Bmf.Api.Boilerplate.Application.Abstractions.Messaging;
using Bmf.Api.Boilerplate.Application.Ports;

namespace Bmf.Api.Boilerplate.Application.Behaviors;

/// <summary>Evaluates authorization when the request declares a policy.</summary>
public sealed class AuthorizationBehavior<TRequest, TResponse>(IAuthorizationService authorization, IUserContext user)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (request is IAuthorizableRequest authorizable)
        {
            await authorization.EnsureAuthorizedAsync(user, authorizable.Policy, request!, ct).ConfigureAwait(false);
        }

        return await next().ConfigureAwait(false);
    }
}
