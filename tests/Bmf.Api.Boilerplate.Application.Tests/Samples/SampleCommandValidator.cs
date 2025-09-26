using FluentValidation;

namespace Bmf.Api.Boilerplate.Application.Tests.Samples;

public sealed class SampleCommandValidator : AbstractValidator<SampleCommand>
{
    public SampleCommandValidator()
    {
        _ = RuleFor(x => x.Payload).NotEmpty();
    }
}
