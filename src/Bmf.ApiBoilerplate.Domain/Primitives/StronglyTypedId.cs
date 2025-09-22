namespace Bmf.ApiBoilerplate.Domain.Primitives;

/// <summary>
/// Base for Guid-backed strongly-typed IDs as reference-type records.
/// Example: <c>public sealed record OrderId(Guid Value) : StronglyTypedId&lt;OrderId&gt;(Value);</c>
/// </summary>
public abstract record StronglyTypedId<TSelf>(Guid Value) where TSelf : StronglyTypedId<TSelf>
{
    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(StronglyTypedId<TSelf> id)
    {
        return id.Value;
    }

    public static bool TryParse(string? input, out Guid value)
    {
        if (Guid.TryParse(input, out Guid g))
        {
            value = g;
            return true;
        }

        value = Guid.Empty;
        return false;
    }
}
