using CSharpFunctionalExtensions;

namespace DiscordSecretSanta.Core.AssignSecretSantas;

public interface IAssignSecretSantas
{
    Task<Result> Run(CancellationToken cancellationToken);
}