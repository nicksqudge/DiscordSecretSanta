using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordSecretSanta.DataStore.Json;

public class JsonDataStore : IDataStore
{
    private const string FileName = "DiscordSecretSanta.json";

    private readonly string _filePath;
    private JsonFile _data = new();

    public JsonDataStore()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
        
        if(!File.Exists(_filePath))
            WriteFile();
        else
            ReadFile();
    }

    public Task<bool> IsAdminInConfig(DiscordUserId userId, CancellationToken cancellationToken)
        => Task.FromResult(_data.Admins.Contains(userId.Value));

    public Task<Status> GetStatus(CancellationToken cancellationToken) => Task.FromResult(_data.Status);

    public Task<SecretSantaConfig> GetConfig(CancellationToken cancellationToken) => Task.FromResult(_data.Config);

    public Task SetStatus(Status status, CancellationToken cancellationToken)
    {
        _data.Status = status;
        WriteFile();
        return Task.CompletedTask;
    }

    public Task<int> GetNumberOfMembers(CancellationToken cancellationToken)
    {
        return Task.FromResult(_data.Members.Length);
    }

    public Task<SecretSantaMember[]> GetMembers(CancellationToken cancellationToken)
    {
        return Task.FromResult(_data.Members.Select(member => new SecretSantaMember(new DiscordUserId(member.DiscordId), new Uri(member.WishlistUrl))).ToArray());
    }

    public Task ToggleAdmin(DiscordUserId userId, bool isAdmin, CancellationToken cancellationToken)
    {
        if (_data.Admins.Contains(userId.Value))
            _data.Admins = _data.Admins.Where(a => a != userId.Value).ToArray();
        else
        {
            var admins = _data.Admins.ToList();
            admins.Add(userId.Value);
            _data.Admins = admins.ToArray();
        }
        
        WriteFile();
        return Task.CompletedTask;
    }

    public Task SetMaxPrice(string newMaxPrice, CancellationToken cancellationToken)
    {
        _data.Config.MaxPrice = newMaxPrice;
        WriteFile();
        return Task.CompletedTask;
    }

    public Task AddMember(DiscordUserId discordUserId, Uri wishlistUrl, CancellationToken cancellationToken)
    {
        var members = _data.Members.ToList();
        members.Add(new JsonFile.Member()
        {
            DiscordId = discordUserId.Value,
            WishlistUrl = wishlistUrl.ToString(),
        });
        _data.Members = members.ToArray();
        WriteFile();
        return Task.CompletedTask;
    }

    public Task SetSecretSanta(DiscordUserId targetUser, DiscordUserId secretSanta, CancellationToken cancellationToken)
    {
        var members = _data.Members.ToList();
        members.FirstOrDefault(m => m.DiscordId == targetUser.Value).SecretSanta = secretSanta.Value;
        _data.Members = members.ToArray();
        WriteFile();
        return Task.CompletedTask;
    }

    public Task<SecretSantaMember?> GetMember(DiscordUserId discordUserId, CancellationToken cancellationToken)
    {
        var member = _data.Members.FirstOrDefault(m => m.DiscordId == discordUserId.Value);
        if (member is null)
            return Task.FromResult<SecretSantaMember?>(null);

        var result = new SecretSantaMember(new DiscordUserId(member.DiscordId), new Uri(member.WishlistUrl));
        return Task.FromResult<SecretSantaMember?>(result);
    }

    private void WriteFile()
    {
        var fileData = JsonSerializer.Serialize(_data);
        File.WriteAllText(_filePath, fileData);
    }

    private void ReadFile()
    {
        var fileData = File.ReadAllText(_filePath);
        _data = JsonSerializer.Deserialize<JsonFile>(fileData) ??  new JsonFile();
    }
}