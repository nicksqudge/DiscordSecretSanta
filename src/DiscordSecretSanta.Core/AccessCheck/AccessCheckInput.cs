namespace DiscordSecretSanta.Core.AccessCheck;

public class AccessCheckInput
{
    public bool MustBeLoggedIn { get; set; }
    public bool MustHaveUserAccount { get; set; }
    public bool MustBeAdmin { get; set; }
}