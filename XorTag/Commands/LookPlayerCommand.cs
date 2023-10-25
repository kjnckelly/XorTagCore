using XorTag.Domain;

namespace XorTag.Commands;

public class LookPlayerCommand
{
    private readonly IPlayerRepository playerRepository;
    private readonly IMapSettings mapSettings;

    public LookPlayerCommand(IPlayerRepository playerRepository, IMapSettings mapSettings)
    {
        this.playerRepository = playerRepository;
        this.mapSettings = mapSettings;
    }

    public CommandResult Execute(int playerId)
    {
        var allPlayers = playerRepository.GetAllPlayers();
        var currentPlayer = allPlayers.SingleOrDefault(x => x.Id == playerId);
        if (currentPlayer == null)
        {
            throw new NotFoundException();

        }
        if (currentPlayer.LastAction.AddSeconds(1) > DateTime.Now)
        {
            currentPlayer.LastAction = DateTime.Now.AddSeconds(1);
            throw new InvalidOperationException();
        }
        currentPlayer.LastAction = DateTime.Now;
        if (currentPlayer == null) throw new NotFoundException();
        return new CommandResult
        {
            X = currentPlayer.X,
            Y = currentPlayer.Y,
            IsIt = currentPlayer.IsIt,
            Players = playerRepository.GetNearbyPlayers(currentPlayer.X, currentPlayer.Y).Select(p => new PlayerResult() { IsIt = p.IsIt, X = p.X, Y = p.Y }).ToList()
        };
    }
}