using CSharpFunctionalExtensions;
using DiscordSecretSanta.Core.Repositories;
using FluentAssertions;
using FluentAssertions.Primitives;
using NSubstitute;

namespace DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;

public class UserRepositoryHelper : DependencyHelper<IUserRepository, UserRepositoryAssertions>
{
    public List<User> Users = new();
    
    protected override UserRepositoryAssertions _assertions => new(Object, Users);

    public UserRepositoryHelper()
    {
        Object.CreateUser(Arg.Any<User>(), default)
            .ReturnsForAnyArgs(args => Result.Success(args[0] as User));

        Object.UpdateSecretSanta(default, default, default, default)
            .ReturnsForAnyArgs(args =>
            {
                var targetUserId = args[0] as UserId;
                var secretSanta = args[1] as UserId;
                var status = (SecretSantaStatus)args[2];

                var user = Users.FirstOrDefault(u => u.UserId.Value == targetUserId.Value);
                if (user is null)
                    return Result.Failure("No such user");

                user.SecretSantaUserId = secretSanta;
                user.SecretSantaStatus = status;

                return Result.Success();
            });

        Object.ListUsers(default)
            .ReturnsForAnyArgs(Users);
    }

    public UserRepositoryHelper HasUser(User user)
    {
        Object.DoesUserExist(default, default)
            .ReturnsForAnyArgs(Task.FromResult(true));
        
        Object.GetUser(default, default)
            .ReturnsForAnyArgs(user);

        Users.Add(user);
        
        return this;
    }

    public UserRepositoryHelper HasUser()
    {
        return HasUser(new User("Test", "1234", "1234", new UserId(new Random().Next(1, 100).ToString())));
    }

    public UserRepositoryHelper HasUser(Action<User> transform)
    {
        var user = new User("Test", "1234", "1234", new UserId(new Random().Next(1, 100).ToString()));
        transform.Invoke(user);
        return HasUser(user);
    }
    
    public UserRepositoryHelper HasNoUser()
    {
        Object.DoesUserExist(default, default)
            .ReturnsForAnyArgs(Task.FromResult(false));

        Object.GetUser(default, default)
            .ReturnsForAnyArgs(Maybe<User>.None);

        Users = new();

        return this;
    }

    public UserRepositoryHelper CountOfUsersIs(int count)
    {
        Object.CountUsers(default)
            .ReturnsForAnyArgs(count);

        return this;
    }
}

public class UserRepositoryAssertions : ReferenceTypeAssertions<IUserRepository, UserRepositoryAssertions>
{
    private readonly List<User> _userList;

    public UserRepositoryAssertions(IUserRepository subject, List<User> userList) : base(subject)
    {
        _userList = userList;
    }

    protected override string Identifier => nameof(UserRepositoryAssertions);

    public AndConstraint<UserRepositoryAssertions> HaveCreatedUser()
    {
        Subject.ReceivedWithAnyArgs().CreateUser(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> HaveFetchedUser()
    {
        Subject.ReceivedWithAnyArgs().GetUser(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> NotHaveCreatedUser()
    {
        Subject.DidNotReceiveWithAnyArgs().CreateUser(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }
    
    public AndConstraint<UserRepositoryAssertions> NotHaveFetchedUser()
    {
        Subject.DidNotReceiveWithAnyArgs().GetUser(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> NotHaveSavedWishlistUrl()
    {
        Subject.DidNotReceiveWithAnyArgs().SaveUserWishlistUrl(default, default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> HaveSavedWishlistUrl()
    {
        Subject.ReceivedWithAnyArgs().SaveUserWishlistUrl(default, default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> MadeUserAdmin()
    {
        Subject.ReceivedWithAnyArgs().MakeUserAdmin(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }
    
    public AndConstraint<UserRepositoryAssertions> NotMadeUserAdmin()
    {
        Subject.DidNotReceiveWithAnyArgs().MakeUserAdmin(default, default);

        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> AllUsersShouldHaveSecretSanta()
    {
        _userList.All(x => 
                x.SecretSantaUserId != null && 
                x.SecretSantaUserId.Value != x.UserId.Value &&
                x.SecretSantaStatus == SecretSantaStatus.Assigned
            ).Should()
            .BeTrue("Not all users have a secret santa id");
        
        return new AndConstraint<UserRepositoryAssertions>(this);
    }

    public AndConstraint<UserRepositoryAssertions> NoneShouldHaveDuplicatesAssigned()
    {
        _userList
            .Where(x => x.SecretSantaUserId != null)
            .Select(x => x.SecretSantaUserId!.Value)
            .ToHashSet()
            .Count().Should().Be(_userList.Count, "There are duplicates in the list");
        
        return new AndConstraint<UserRepositoryAssertions>(this);
    }
}