using System;
using System.Collections.Generic;
using System.Linq;
using XorTag.Domain;

namespace XorTag.Commands
{
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
            if (currentPlayer == null) throw new NotFoundException();
            return new CommandResult
            {
                X = currentPlayer.X,
                Y = currentPlayer.Y,
                IsIt = currentPlayer.IsIt
            };
        }
    }
}