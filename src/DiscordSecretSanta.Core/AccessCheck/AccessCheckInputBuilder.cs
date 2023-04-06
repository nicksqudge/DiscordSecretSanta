namespace DiscordSecretSanta.Core.AccessCheck;

public class AccessCheckInputBuilder
{
    private AccessCheckInput _result = new();
    
    public AccessCheckInputBuilder MustBeLoggedIn()
    {
        _result.MustBeLoggedIn = true;
        return this;
    }

    public AccessCheckInputBuilder MustHaveUser()
    {
        MustBeLoggedIn();
        _result.MustHaveUserAccount = true;
        return this;
    }

    public AccessCheckInputBuilder MustBeAdmin()
    {
        MustBeLoggedIn();
        MustHaveUser();
        _result.MustBeAdmin = true;
        return this;
    }

    public AccessCheckInput Build()
    {
        return _result;
    }
}