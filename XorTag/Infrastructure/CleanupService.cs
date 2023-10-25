
using System.Threading;
using XorTag.Domain;

namespace XorTag.Infrastructure;

public class CleanupService : BackgroundService
{
    private IPlayerRepository playerRepository;

    public CleanupService(IPlayerRepository playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
        {
            this.playerRepository.ClearDeadPlayers();
        }
    }
}