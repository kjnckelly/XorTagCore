using XorTag.Domain;

namespace XorTag.Commands;

public class RegisterPlayerCommand
{
    private readonly IIdGenerator idGenerator;
    private readonly INameGenerator nameGenerator;
    private readonly IMapSettings mapSettings;
    private readonly IRandom random;
    private readonly IPlayerRepository playerRepository;

    public RegisterPlayerCommand(IIdGenerator idGenerator, INameGenerator nameGenerator, IMapSettings mapSettings, IRandom random, IPlayerRepository playerRepository)
    {
        this.idGenerator = idGenerator;
        this.nameGenerator = nameGenerator;
        this.mapSettings = mapSettings;
        this.random = random;
        this.playerRepository = playerRepository;
    }
    public RegistrationResult Execute(string? name)
    {
        var playerCount = playerRepository.GetPlayerCount();
        var player = new Player
        {
            Id = idGenerator.GenerateId(new int[] { }),
            Name = name ?? nameGenerator.GenerateName(playerRepository.GetAllPlayers().Select(p => p.Name)),
            X = random.Next(mapSettings.MapWidth),
            Y = random.Next(mapSettings.MapHeight),
            LastAction = DateTime.Now,
            LastMove = DateTime.Now,
        IsIt = playerCount == 0

        };
        playerRepository.Save(player);
        return new RegistrationResult()
        {
            Name = player.Name,
            Id = player.Id,
            IsIt = player.IsIt,
            MapWidth = mapSettings.MapWidth,
            MapHeight = mapSettings.MapHeight,
            X = player.X,
            Y = player.Y,
            Players = playerRepository.GetNearbyPlayers(player.X, player.Y).Select(p => new PlayerResult() { IsIt = p.IsIt, X = p.X, Y = p.Y }).ToList()
        };
    }
}