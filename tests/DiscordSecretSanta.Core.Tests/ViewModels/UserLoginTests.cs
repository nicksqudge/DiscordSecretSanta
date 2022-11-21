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

    private readonly UserLoginViewHandler _handler;

    private readonly ISetupService _setupService = Substitute.For<ISetupService>();

    public UserLoginTests()
    {
        _setupService
            .GetTitle()
            .ReturnsForAnyArgs(ExpectedTitle);
        
        _handler = new UserLoginViewHandler(_setupService);
    }
    
    [Fact]
    public async void NoLoggedInUser()
    {
        var result = await _handler.OnInitAsync();
        result.Title.Should().Be(ExpectedTitle);
        result.User.Should().BeNull();
        result.WishlistUrl.Should().BeEmpty();
    }

    [Fact]
    public async void UserLoggedIn_NoAmazonWishlist()
    {
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
    }

    [Fact]
    public async void UserLoggedIn_WithAmazonWishlist()
    {
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
    }
}