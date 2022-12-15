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

    private readonly UserLoginViewHandler _handler;

    private readonly ISetupService _setupService = Substitute.For<ISetupService>();
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserLoginTests()
    {
        _setupService
            .GetTitle()
            .ReturnsForAnyArgs(ExpectedTitle);
        
        _handler = new UserLoginViewHandler(_setupService, _userService);
    }
    
    [Fact]
    public async void NoLoggedInUser()
    {
        _userService.GetCurrentUser()
            .Returns(Task.FromResult<User>(null));
        
        var result = await _handler.OnInitAsync();
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.WishlistUrl.Should().BeEmpty();
        result.HasUser.Should().BeFalse();
        result.HasWishlist.Should().BeFalse();
    }

    [Fact]
    public async void UserLoggedIn_NoAmazonWishlist()
    {
        _userService.GetCurrentUser()
            .Returns(new User(ExpectedUserName, ExpectedDiscordTagId, ExpectedAvatarId, string.Empty, ExpectedUserId));
        
        var result = await _handler.OnInitAsync();
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserLoginViewModel.CurrentUser()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId
            });
        result.WishlistUrl.Should().BeEmpty();
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeFalse();
    }

    [Fact]
    public async void UserLoggedIn_WithAmazonWishlist()
    {
        _userService.GetCurrentUser()
            .Returns(new User(ExpectedUserName, ExpectedDiscordTagId, ExpectedAvatarId, ExpectedWishlistUrl, ExpectedUserId));
        
        var result = await _handler.OnInitAsync();
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserLoginViewModel.CurrentUser()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId
            });
        result.WishlistUrl.Should().Be(ExpectedWishlistUrl);
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeTrue();
    }
}