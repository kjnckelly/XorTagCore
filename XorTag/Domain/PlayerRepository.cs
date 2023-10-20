namespace XorTag.Domain;

public interface IPlayerRepository
{
    IEnumerable<Player> GetAllPlayers();
    void UpdatePlayerPosition(Player player);
    void SavePlayerAsIt(int playerId);
    void SavePlayerAsNotIt(int playerId);
    void Save(Player player);
    int GetPlayerCount();
    void ClearAllPlayers();
    IEnumerable<Player> GetNearbyPlayers(int X, int Y);
}

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> players = new List<Player>();

    public void ClearAllPlayers()
    {
        players.Clear();
    }
    public IEnumerable<Player> GetAllPlayers()
    {
        return players;
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    public void Save(Player player)
    {
        var playerCopy = new Player
        {
            Id = player.Id,
            Name = player.Name,
            X = player.X,
            Y = player.Y,
            IsIt = player.IsIt
        };
        players.Add(playerCopy);
    }

    public void SavePlayerAsIt(int playerId)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == playerId);
        if (playerToUpdate != null) playerToUpdate.IsIt = true;
    }

    public void SavePlayerAsNotIt(int playerId)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == playerId);
        if (playerToUpdate != null) playerToUpdate.IsIt = false;
    }

    public void UpdatePlayerPosition(Player player)
    {
        var playerToUpdate = players.FirstOrDefault(x => x.Id == player.Id);
        if (playerToUpdate == null) return;
        playerToUpdate.X = player.X;
        playerToUpdate.Y = player.Y;
    }
    
    public IEnumerable<Player> GetNearbyPlayers(int X, int Y)
        => players.Where(p => InRange(Math.Abs(X - p.X) + Math.Abs(Y - p.Y),1 ,15));

    private bool InRange(int val, int min, int max) => val >= min && val <= max;

}