using Bmf.Api.Boilerplate.Domain.Primitives;
using FluentAssertions;
using Xunit;

namespace Bmf.Api.Boilerplate.Domain.Tests.Primitives;

public sealed record AggId(Guid Value) : StronglyTypedId<AggId>(Value);

public sealed class TestOccurred : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed class TestAggregate(AggId id) : AggregateRoot<AggId>(id)
{
    public void DoSomething()
    {
        Raise(new TestOccurred());
    }
}

public class AggregateDomainEventTests
{
    [Fact]
    public void Raise_and_clear_events()
    {
        TestAggregate agg = new(new AggId(Guid.NewGuid()));
        _ = agg.DomainEvents.Should().BeEmpty();
        agg.DoSomething();
        _ = agg.DomainEvents.Should().HaveCount(1).And.ContainItemsAssignableTo<IDomainEvent>();
        agg.ClearDomainEvents();
        _ = agg.DomainEvents.Should().BeEmpty();
    }
}
