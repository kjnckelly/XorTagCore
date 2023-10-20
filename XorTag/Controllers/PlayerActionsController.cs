using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers;

[ApiController]
public class PlayerActionsController : ControllerBase
{
    private readonly RegisterPlayerCommand registerPlayerCommand;
    private readonly MovePlayerCommand movePlayerCommand;
    private readonly LookPlayerCommand lookPlayerCommand;

    public PlayerActionsController(RegisterPlayerCommand registerPlayerCommand, MovePlayerCommand movePlayerCommand, LookPlayerCommand lookPlayerCommand)
    {
        this.registerPlayerCommand = registerPlayerCommand;
        this.movePlayerCommand = movePlayerCommand;
        this.lookPlayerCommand = lookPlayerCommand;
    }

    [HttpPost("/register")]
    public RegistrationResult Register()
    {
        return registerPlayerCommand.Execute();
    }

    [HttpPost("/move{direction}/{playerId}")]
    public IActionResult Move(string direction, int playerId)
    {
        try
        {
            return Ok(movePlayerCommand.Execute(direction, playerId));
        }
        catch(InvalidOperationException)
        {
            return StatusCode(429);
        }
    }

    [HttpGet("/look/{playerId}")]
    public IActionResult Look(int playerId)
    {
        try
        {
            return Ok(lookPlayerCommand.Execute(playerId));
        }
        catch (InvalidOperationException)
        {
            return StatusCode(429);
        }
    }
}