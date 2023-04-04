using DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;
using DiscordSecretSanta.Core.ViewModels;
using DiscordSecretSanta.Core.ViewModels.UserLogin;
using FluentAssertions;
using NSubstitute;

namespace DiscordSecretSanta.Core.Tests.ViewModels;

public class UserLoginTests
{
    private const string ExpectedTitle = "A Test Campaign";
    private const string ExpectedAvatarId = "123456789";
    private const string ExpectedUserName = "thornphoenix";
    private const string ExpectedDiscordTagId = "4567";
    private const string ExpectedWishlistUrl = "http://www.amazon.co.uk/wishlist/123";
    private const string ExpectedUserId = "ABCASDSAD";

    private AuthenticatedUser ExpectedAuthUser = new AuthenticatedUser(ExpectedUserName, ExpectedDiscordTagId,
        ExpectedAvatarId, new UserId(ExpectedUserId));

    private User ExpectedUser = new User(ExpectedUserName, ExpectedDiscordTagId,
        ExpectedAvatarId, new UserId(ExpectedUserId))
    {
        WishlistUrl = !string.IsNullOrWhiteSpace(ExpectedWishlistUrl) ? new Uri(ExpectedWishlistUrl) : null
    };

    private readonly UserLoginViewHandler _handler;

    private readonly ISetupService _setupService = Substitute.For<ISetupService>();
    private readonly AuthProviderServiceHelper _authProviderService = new();
    private readonly UserRepositoryHelper _userRepository = new();

    public UserLoginTests()
    {
        _setupService
            .GetTitle()
            .ReturnsForAnyArgs(ExpectedTitle);
        
        _handler = new UserLoginViewHandler(
            _setupService, 
            _authProviderService.Object, 
            _userRepository.Object);
    }
    
    [Fact]
    public async void NoLoggedInUser()
    {
        _authProviderService.HasNoUser();
        _userRepository.HasNoUser();
        
        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.HasUser.Should().BeFalse();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeFalse();

        _userRepository.Should().NotHaveCreatedUser()
            .And.NotHaveFetchedUser();
    }

    [Fact]
    public async void UserLoggedIn_UserExists_NoAmazonWishlist()
    {
        _authProviderService.ReturnsGetCurrentUser(ExpectedAuthUser);
        ExpectedUser.WishlistUrl = null;
        _userRepository
            .HasUser(ExpectedUser)
            .CountOfUsersIs(10);

        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserViewModel()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId,
                WishlistUrl = string.Empty
            });
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeFalse();

        _userRepository.Should().NotHaveCreatedUser()
            .And.HaveFetchedUser();
    }
    
    [Fact]
    public async void UserLoggedIn_UserDoesNotExist_NoAmazonWishlist()
    {
        _authProviderService.ReturnsGetCurrentUser(ExpectedAuthUser);
        _userRepository.HasNoUser()
            .CountOfUsersIs(1);

        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserViewModel()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId,
                IsAdmin = true,
                WishlistUrl = string.Empty
            });
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeFalse();

        _userRepository.Should().HaveCreatedUser()
            .And.NotHaveFetchedUser()
            .And.MadeUserAdmin();
    }

    [Fact]
    public async void UserLoggedIn_WithAmazonWishlist()
    {
        _authProviderService.ReturnsGetCurrentUser(ExpectedAuthUser);
        _userRepository.HasUser(ExpectedUser)
            .CountOfUsersIs(2);
        
        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserViewModel()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId,
                IsAdmin = false,
                WishlistUrl = ExpectedWishlistUrl
            });
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeTrue();
        result.HasError.Should().BeFalse();

        _userRepository.Should().HaveFetchedUser()
            .And.NotHaveCreatedUser()
            .And.NotMadeUserAdmin();
    }

    [Fact]
    public async void UserNotLoggedIn_UpdateWishListUrl()
    {
        string url = "a test url";
        _authProviderService.HasNoUser();
        _userRepository.HasNoUser().CountOfUsersIs(2);

        var result = await _handler.SetWishlistUrl(url, CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.HasUser.Should().BeFalse();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeTrue();
        result.ErrorMessage.Should().Be("NotLoggedIn");

        _userRepository.Should()
            .NotHaveCreatedUser()
            .And.NotHaveCreatedUser()
            .And.NotHaveSavedWishlistUrl();
    }
    
    [Fact]
    public async void UserLoggedIn_UpdateWishListUrl()
    {
        _authProviderService.ReturnsGetCurrentUser(ExpectedAuthUser);
        _userRepository.HasUser(ExpectedUser).CountOfUsersIs(2);
        string url = "https://amazon.co.uk/wishlist/1234";

        var result = await _handler.SetWishlistUrl(url, CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserViewModel()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId,
                IsAdmin = false,
                WishlistUrl = url
            });
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeTrue();
        result.HasError.Should().BeFalse();

        _userRepository.Should()
            .HaveSavedWishlistUrl()
            .And.NotHaveCreatedUser();
    }
}