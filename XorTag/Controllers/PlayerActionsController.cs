using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;
using XorTag.Domain;

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
    public RegistrationResult Register(string? name = null)
    {
        if (name == "{{your name}}") name = null;
        return registerPlayerCommand.Execute(name);
    }

    [HttpPost("/move{direction}/{playerId}")]
    public IActionResult Move(string direction, int playerId)
    {
        try
        {
            return Ok(movePlayerCommand.Execute(direction, playerId));
        }
        catch (NotFoundException)
        {
            return NotFound("Player ID is not currently registered");
        }
        catch (InvalidOperationException)
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
        catch (NotFoundException)
        {
            return NotFound("Player ID is not currently registered");
        }
        catch (InvalidOperationException)
        {
            return StatusCode(429);
        }
    }
}