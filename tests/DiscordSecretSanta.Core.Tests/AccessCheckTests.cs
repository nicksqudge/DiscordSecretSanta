using DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;
using FluentAssertions;

namespace DiscordSecretSanta.Core.Tests;

public class AccessCheckTests
{
    private readonly AccessCheck _target;
    private readonly UserRepositoryHelper _userRepository = new();
    private readonly AuthProviderServiceHelper _authProviderService = new();

    private readonly AuthenticatedUser _authenticatedUser =
        new AuthenticatedUser("Test", "1234", "1234", new UserId("1234"));
    
    public AccessCheckTests()
    {
        _target = new AccessCheck(
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
        _authProviderService.ReturnsGetCurrentUser(_authenticatedUser);
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserIsLoggedInButHasNoUser_IsRequiredToHaveAUser_CanNotAccess()
    {
        _authProviderService.ReturnsGetCurrentUser(_authenticatedUser);
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
        _authProviderService.ReturnsGetCurrentUser(_authenticatedUser);
        _userRepository.HasUser(_authenticatedUser.ToUser());
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task UserIsLoggedInAndIsNotAnAdmin_IsRequiredToBeAdmin_CanNotAccess()
    {
        _authProviderService.ReturnsGetCurrentUser(_authenticatedUser);
        var user = _authenticatedUser.ToUser();
        user.IsAdmin = false;
        _userRepository.HasUser(user);
        
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
        _authProviderService.ReturnsGetCurrentUser(_authenticatedUser);
        var user = _authenticatedUser.ToUser();
        user.IsAdmin = true;
        _userRepository.HasUser(user);
        
        var checks = new AccessCheckInputBuilder()
            .MustBeLoggedIn()
            .MustHaveUser()
            .MustBeAdmin();

        var result = await _target.CanAccess(checks.Build(), CancellationToken.None);

        result.Should().BeTrue();
    }
}