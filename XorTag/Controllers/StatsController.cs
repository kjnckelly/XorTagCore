using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class StatsController : ControllerBase
{
    private readonly StatsCommand statsCommand;

    public StatsController(StatsCommand statsCommand)
    {
        this.statsCommand = statsCommand;
    }

    [HttpGet("/stats")]
    public StatsResult Get()
    {
        return statsCommand.Execute();
    }
}