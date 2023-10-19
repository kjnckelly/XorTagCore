using Microsoft.AspNetCore.Mvc;
using XorTag.Commands;

namespace XorTag.Controllers
{
    [ApiController]
    public class PlayerActionsController: ControllerBase
    {
        private readonly RegisterPlayerCommand registerPlayerCommand;
        private readonly MovePlayerCommand movePlayerCommand;
        private readonly LookPlayerCommand lookPlayerCommand;

        public PlayerActionsController(RegisterPlayerCommand registerPlayerCommand, MovePlayerCommand movePlayerCommand)
        {
            this.registerPlayerCommand = registerPlayerCommand;
            this.movePlayerCommand = movePlayerCommand;
        }

        [HttpPost("/register")]
        public CommandResult Register()
        {
            return registerPlayerCommand.Execute();
        }

        [HttpPost("/move{direction}/{playerId}")]
        public CommandResult Move(string direction, int playerId)
        {
            return movePlayerCommand.Execute(direction, playerId);
        }

        [HttpGet("/look/{playerId}")]
        public CommandResult Look(int playerId)
        {
            return lookPlayerCommand.Execute(playerId);
        }

        //TODO - implement Look command
    }
}