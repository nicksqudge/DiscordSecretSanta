namespace DiscordSecretSanta.Json;

public class JsonFile
{
    public Status Status { get; set; } = Status.Ready;
    public Member[] Members { get; set; } = [];

    public class Member
    {
        public string DiscordId { get; set; }
        public string WishlistUrl { get; set; }
    }
}