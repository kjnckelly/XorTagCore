using Microsoft.AspNetCore.Mvc;
using XorTag.Domain;

namespace XorTag.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("/admin")]
public class AdminController : ControllerBase
{
    private readonly IPlayerRepository playerRepository;

    public AdminController(IPlayerRepository playerRepository)
    {
        this.playerRepository = playerRepository;
    }

    [HttpPost("clearall")]
    public void ClearAll()
    {
        playerRepository.ClearAllPlayers();
    }

    [HttpPost("cleanup")]
    public void ClearDeadPlayers()
    {
        playerRepository.ClearDeadPlayers();
    }
}