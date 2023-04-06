using DiscordSecretSanta.Core.AssignSecretSantas;
using DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;
using FluentAssertions;

namespace DiscordSecretSanta.Core.Tests;

public class AssignSecretSantaTests
{
    private readonly UserRepositoryHelper _userRepository = new();
    private readonly AssignSecretSanta _target;

    public AssignSecretSantaTests()
    {
        _target = new(_userRepository.Object);
    }

    [Fact]
    public async Task OnlyHasOneUser_ReturnFalse()
    {
        _userRepository.HasUser();

        var result = await _target.Run(CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task TwoUsers_ReturnsTrue()
    {
        _userRepository.HasUser();
        _userRepository.HasUser();

        var result = await _target.Run(CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _userRepository.Should().AllUsersShouldHaveSecretSanta()
            .And.NoneShouldHaveDuplicatesAssigned();
    }

    [Fact]
    public async Task ThreeUsers_ReturnsTrue()
    {
        _userRepository.HasUser();
        _userRepository.HasUser();
        _userRepository.HasUser();

        var result = await _target.Run(CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        _userRepository.Should().AllUsersShouldHaveSecretSanta();
    }
}