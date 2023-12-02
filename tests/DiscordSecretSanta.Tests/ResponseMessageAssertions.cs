using System.Net;
using FluentAssertions.Primitives;

namespace DiscordSecretSanta.Tests;

public class ResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, ResponseMessageAssertions>
{
    public ResponseMessageAssertions(HttpResponseMessage subject) : base(subject)
    {
    }

    protected override string Identifier => nameof(ResponseMessageAssertions);

    public AndConstraint<ResponseMessageAssertions> HaveStatusCode(HttpStatusCode code)
    {
        Subject.StatusCode.Should().Be(code);
        return new AndConstraint<ResponseMessageAssertions>(this);
    }

    public async Task<AndConstraint<ResponseMessageAssertions>> HaveContent(string expected)
    {
        var content = await Subject.Content.ReadAsStringAsync();
        content.Should().Be(expected);
        return new AndConstraint<ResponseMessageAssertions>(this);
    }
}

public static class ResponseMessageAssertionExtensions
{
    public static AndConstraint<ResponseMessageAssertions> BeOk(this ResponseMessageAssertions assertions)
    {
        return assertions.HaveStatusCode(HttpStatusCode.OK);
    }
}