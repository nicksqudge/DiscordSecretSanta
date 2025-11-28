using System.IO;
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
    
    public Task<Status> GetStatus(CancellationToken cancellationToken)
    {
        return Task.FromResult(_data.Status);
    }

    public Task<SecretSantaConfig> GetConfig(CancellationToken cancellationToken)
    {
        return Task.FromResult(_data.Config);
    }

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

    private void WriteFile()
    {
        var fileData = JsonSerializer.Serialize(_data);
        File.WriteAllText(_filePath, fileData);
    }

    private void ReadFile()
    {
        var fileData = File.ReadAllText(_filePath);
        _data = JsonSerializer.Deserialize<JsonFile>(fileData);
    }
}