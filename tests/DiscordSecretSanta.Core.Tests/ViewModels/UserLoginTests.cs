using DiscordSecretSanta.Core.Repositories;
using DiscordSecretSanta.Core.ViewModels.UserLogin;
using FluentAssertions;
using FluentResults;
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

        _userService.UpdateWishlistUrl(Arg.Any<UserId>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(Task.FromResult(Result.Ok()));

        _userService.GetCurrentUser(default)
            .ReturnsForAnyArgs(Task.FromResult(new User(
                ExpectedUserName, 
                ExpectedDiscordTagId, 
                ExpectedAvatarId,
                new UserId(ExpectedUserId))
            {
                WishlistUrl = new Uri(ExpectedWishlistUrl)
            }));
    }
    
    [Fact]
    public async void NoLoggedInUser()
    {
        UserServiceReturnsNoUser();
        
        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.WishlistUrl.Should().BeEmpty();
        result.HasUser.Should().BeFalse();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeFalse();
    }

    [Fact]
    public async void UserLoggedIn_NoAmazonWishlist()
    {
        _userService.GetCurrentUser(CancellationToken.None)
            .Returns(new User(ExpectedUserName, ExpectedDiscordTagId, ExpectedAvatarId, new UserId(ExpectedUserId)));
        
        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserLoginViewModel.CurrentUser()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId
            });
        result.WishlistUrl.Should().BeEmpty();
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeFalse();
    }

    [Fact]
    public async void UserLoggedIn_WithAmazonWishlist()
    {
        UserServiceReturnsExpectedUser();
        
        var result = await _handler.OnInitAsync(CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserLoginViewModel.CurrentUser()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId
            });
        result.WishlistUrl.Should().Be(ExpectedWishlistUrl);
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeTrue();
        result.HasError.Should().BeFalse();
    }

    [Fact]
    public async void UserNotLoggedIn_UpdateWishListUrl()
    {
        string url = "a test url";

        UserServiceReturnsNoUser();

        var result = await _handler.SetWishlistUrl(url, CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.WishlistUrl.Should().BeEmpty();
        result.HasUser.Should().BeFalse();
        result.HasWishlist.Should().BeFalse();
        result.HasError.Should().BeTrue();
        result.ErrorMessage.Should().Be("NotLoggedIn");
        
        await _userService
            .DidNotReceiveWithAnyArgs()
            .UpdateWishlistUrl(Arg.Any<UserId>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async void UserLoggedIn_UpdateWishListUrl()
    {
        string url = "https://amazon.co.uk/wishlist/1234";
        
        UserServiceReturnsExpectedUser();

        var result = await _handler.SetWishlistUrl(url, CancellationToken.None);
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should()
            .NotBeNull().And
            .BeEquivalentTo(new UserLoginViewModel.CurrentUser()
            {
                AvatarId = ExpectedAvatarId,
                Name = ExpectedUserName,
                DiscordTagId = ExpectedDiscordTagId,
                UserId = ExpectedUserId
            });
        result.WishlistUrl.Should().Be(url);
        result.HasUser.Should().BeTrue();
        result.HasWishlist.Should().BeTrue();
        result.HasError.Should().BeFalse();

        await _userService
            .ReceivedWithAnyArgs()
            .UpdateWishlistUrl(Arg.Any<UserId>(), Arg.Any<Uri>(), Arg.Any<CancellationToken>());
    }

    private void UserServiceReturnsExpectedUser()
    {
        _userService.GetCurrentUser(CancellationToken.None)
            .Returns(new User(ExpectedUserName, 
                ExpectedDiscordTagId, 
                ExpectedAvatarId,
                new UserId(ExpectedUserId))
                {
                    WishlistUrl = new Uri(ExpectedWishlistUrl)
                }
            );
    }

    private void UserServiceReturnsNoUser()
    {
        _userService.GetCurrentUser(CancellationToken.None)
            .Returns(Task.FromResult<User>(null));
    }
}