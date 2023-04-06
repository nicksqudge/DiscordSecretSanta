using DiscordSecretSanta.Core.AccessCheck;
using DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;
using FluentAssertions;

namespace DiscordSecretSanta.Core.Tests;

public class AccessCheckTests
{
    private readonly AccessCheck.AccessCheck _target;
    private readonly UserRepositoryHelper _userRepository = new();
    private readonly AuthProviderServiceHelper _authProviderService = new();

    public AccessCheckTests()
    {
        _target = new AccessCheck.AccessCheck(
            _userRepository.Object,
            _authProviderService.Object
        );
    }
    
    [Fact]
    public async Task UserIsNotLoggedIn_AndIsntRequiredToBe_CanAccess()
    {
        _authProviderService.ReturnsNoCurrentUser();

        var checks = new AccessCheckInputBuilder();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserIsNotLoggedIn_AndIsRequiredToBe_CanNotAccess()
    {
        _authProviderService.ReturnsNoCurrentUser();
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserIsLoggedIn_AndIsRequiredToBe_CanAccess()
    {
        _authProviderService.ReturnsGetCurrentUser();
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserIsLoggedInButHasNoUser_IsRequiredToHaveAUser_CanNotAccess()
    {
        _authProviderService.ReturnsGetCurrentUser();
        _userRepository.HasNoUser();
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserIsLoggedInAndHasUser_IsRequiredToHaveOne_CanAccess()
    {
        _authProviderService.ReturnsGetCurrentUser();
        _userRepository.HasUser();
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserIsLoggedInAndIsNotAnAdmin_IsRequiredToBeAdmin_CanNotAccess()
    {
        _authProviderService.ReturnsGetCurrentUser();
        _userRepository.HasUser(user => user.IsAdmin = false);
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser()
            .MustBeAdmin();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UserIsLoggedInAndIsAdmin_IsRequiredToBeAdmin_CanAccess()
    {
        _authProviderService.ReturnsGetCurrentUser();
        _userRepository.HasUser(user => user.IsAdmin = true);
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser()
            .MustBeAdmin();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }
}