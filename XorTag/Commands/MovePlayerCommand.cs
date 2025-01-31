using System;
using System.Collections.Generic;
using System.Linq;
using XorTag.Domain;

namespace XorTag.Commands
{
    public class MovePlayerCommand
    {
        private readonly IPlayerRepository playerRepository;
        private readonly IMapSettings mapSettings;

        public MovePlayerCommand(IPlayerRepository playerRepository, IMapSettings mapSettings)
        {
            this.playerRepository = playerRepository;
            this.mapSettings = mapSettings;
        }

        public CommandResult Execute(string direction, int playerId)
        {
            var allPlayers = playerRepository.GetAllPlayers();
            var currentPlayer = allPlayers.SingleOrDefault(x => x.Id == playerId);
            if (currentPlayer == null) throw new NotFoundException();
            AdjustPlayerPosition(currentPlayer, direction, allPlayers);
            playerRepository.UpdatePlayerPosition(currentPlayer);
            return new CommandResult
            {
                X = currentPlayer.X,
                Y = currentPlayer.Y,
                IsIt = currentPlayer.IsIt
            };
        }

        private void AdjustPlayerPosition(Player currentPlayer, string direction, IEnumerable<Player> allPlayers)
        {
            int newX = currentPlayer.X;
            int newY = currentPlayer.Y;
            switch (direction)
            {
                case "up":
                    newY -= 1;
                    break;
                case "down":
                    newY += 1;
                    break;
                case "left":
                    newX -= 1;
                    break;
                case "right":
                    newX += 1;
                    break;
                default:
                    throw new NotFoundException();
            }
            if (IsNewPositionValid(newX, newY, allPlayers))
            {
                var playerAtNewPosition = GetPlayerAtPosition(newX, newY, allPlayers);
                if (playerAtNewPosition == null)
                {
                    currentPlayer.X = newX;
                    currentPlayer.Y = newY;
                }
                else
                {
                    UpdateIsItStatus(currentPlayer, playerAtNewPosition);
                }
            }
        }

        private void UpdateIsItStatus(Player currentPlayer, Player playerAtNewPosition)
        {
            if (currentPlayer.IsIt)
            {
                currentPlayer.IsIt = false;
                playerRepository.SavePlayerAsNotIt(currentPlayer.Id);
                playerRepository.SavePlayerAsIt(playerAtNewPosition.Id);
            }
            else if (playerAtNewPosition.IsIt)
            {
                currentPlayer.IsIt = true;
                playerRepository.SavePlayerAsIt(currentPlayer.Id);
                playerRepository.SavePlayerAsNotIt(playerAtNewPosition.Id);
            }
        }

        private bool IsNewPositionValid(int newX, int newY, IEnumerable<Player> allPlayers)
        {
            if (
                newY < 0 || newY >= mapSettings.MapHeight ||
                newX < 0 || newX >= mapSettings.MapWidth)
            {
                return false;
            }
            return true;
        }

        private static Player GetPlayerAtPosition(int newX, int newY, IEnumerable<Player> allPlayers)
        {
            return allPlayers.FirstOrDefault(p => p.X == newX && p.Y == newY);
        }

    }
}