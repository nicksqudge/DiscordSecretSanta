﻿using CSharpFunctionalExtensions;
using DiscordSecretSanta.Core.Repositories;
using FluentAssertions;
using FluentAssertions.Primitives;
using NSubstitute;

namespace DiscordSecretSanta.Core.Tests.TestHelpers.DepedencyAssertions;

public class UserRepositoryHelper : DependencyHelper<IUserRepository, UserRepositoryAssertions>
{
    protected override UserRepositoryAssertions _assertions => new(Object);

    public UserRepositoryHelper()
    {
        Object.CreateUser(Arg.Any<User>(), default)
            .ReturnsForAnyArgs(args => Result.Success(args[0] as User));
    }

    public UserRepositoryHelper HasUser(User user)
    {
        Object.DoesUserExist(default, default)
            .ReturnsForAnyArgs(Task.FromResult(true));
        
        Object.GetUser(default, default)
            .ReturnsForAnyArgs(user);

        return this;
    }
    
    public UserRepositoryHelper HasNoUser()
    {
        Object.DoesUserExist(default, default)
            .ReturnsForAnyArgs(Task.FromResult(false));

        Object.GetUser(default, default)
            .ReturnsForAnyArgs(Maybe<User>.None);

        return this;
    }
}

public class UserRepositoryAssertions : ReferenceTypeAssertions<IUserRepository, UserRepositoryAssertions>
{
    public UserRepositoryAssertions(IUserRepository subject) : base(subject)
    {
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
}