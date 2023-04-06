using System.Xml.Schema;
using CSharpFunctionalExtensions;
using DiscordSecretSanta.Core.Repositories;

namespace DiscordSecretSanta.Core.AssignSecretSantas;

public class AssignSecretSanta : IAssignSecretSantas
{
    private readonly IUserRepository _userRepository;
    private readonly Random _random = new();

    public AssignSecretSanta(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Run(CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListUsers(cancellationToken);
        if (users.Count() <= 1)
            return Result.Failure("InvalidNumberOfUsers");
        
        var resultUsers = users
            .Select(x => x.UserId)
            .ToArray();

        var remainingUsers = users
            .Select(x => x.UserId)
            .ToList();
        
        foreach (var user in resultUsers)
        {
            var pool = remainingUsers.Where(x => x.Value != user.Value)
                .ToList();

            UserId otherUserId;
            if (pool.Count() == 1)
                otherUserId = pool.First();
            else
            {
                int otherUserIndex = _random.Next(0, pool.Count() - 1);
                otherUserId = pool[otherUserIndex];
            }

            var result = await _userRepository.SetSecretSanta(user, otherUserId, cancellationToken);
            if (result.IsFailure)
                return result;
        }
        
        return Result.Success();
    }
}