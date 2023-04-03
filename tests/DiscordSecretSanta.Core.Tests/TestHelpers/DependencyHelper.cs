using FluentAssertions.Primitives;
using NSubstitute;

namespace DiscordSecretSanta.Core.Tests.TestHelpers;

public abstract class DependencyHelper<T, TAssertions>
    where T : class
    where TAssertions : ReferenceTypeAssertions<T, TAssertions>
{
    public T Object { get; } = Substitute.For<T>();
    protected abstract TAssertions _assertions { get; }

    public TAssertions Should()
    {
        return _assertions;
    }
}