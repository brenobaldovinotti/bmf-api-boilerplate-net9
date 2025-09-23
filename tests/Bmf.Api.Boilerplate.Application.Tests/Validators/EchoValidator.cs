using Bmf.Api.Boilerplate.Application.Tests.Requests;
using FluentValidation;

namespace Bmf.Api.Boilerplate.Application.Tests.Validators;

public sealed class EchoValidator : AbstractValidator<Echo>
{
    public EchoValidator()
    {
        _ = RuleFor(x => x.Message).NotEmpty();
    }
}
