using NetArchTest.Rules;
using Xunit;

namespace Bmf.Api.Boilerplate.Architecture.Tests;

public class DomainArchitectureTests
{
    [Fact]
    public void Domain_should_not_depend_on_other_layers()
    {
        TestResult result = Types
            .InAssembly(typeof(Domain.Abstractions.Error).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Bmf.Api.Boilerplate.API",
                "Bmf.Api.Boilerplate.Application",
                "Bmf.Api.Boilerplate.Infrastructure")
            .GetResult();

        if (!result.IsSuccessful)
        {
            string details = result.FailingTypeNames is null
                ? "Unknown failing types."
                : string.Join(Environment.NewLine, result.FailingTypeNames);

            throw new Xunit.Sdk.XunitException(
                $"Domain has forbidden dependencies:{Environment.NewLine}{details}");
        }
    }
}
