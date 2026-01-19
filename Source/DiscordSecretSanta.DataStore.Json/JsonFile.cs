namespace DiscordSecretSanta.DataStore.Json;

public class JsonFile
{
    public Status Status { get; set; } = Status.Ready;
    public Member[] Members { get; set; } = [];
    public SecretSantaConfig Config { get; set; } = new ();
    public ulong[] Admins  { get; set; } = [];

    public class Member
    {
        public ulong DiscordId { get; set; }
        public string WishlistUrl { get; set; } = string.Empty;
    }
}