namespace DiscordSecretSanta.DataStore.Json;

public class JsonFile
{
    public Status Status { get; set; } = Status.Ready;
    public Member[] Members { get; set; } = [];
    public SecretSantaConfig Config { get; set; } = new ();

    public class Member
    {
        public string DiscordId { get; set; }
        public string WishlistUrl { get; set; }
    }
}