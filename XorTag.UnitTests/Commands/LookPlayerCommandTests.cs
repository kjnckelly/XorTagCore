using System.Collections.Generic;
using NUnit.Framework;
using XorTag.Commands;
using XorTag.Domain;

namespace XorTag.UnitTests.Commands;

public class LookPlayerCommandTests
{
    private const int mapHeight = 30;
    private const int mapWidth = 50;

    public class When_looking : WithAnAutomocked<LookPlayerCommand>
    {
        private CommandResult result;
        private const int playerStartY = 12;
        private const int playerStartX = 23;
        private Player player;
        private List<Player> allPlayers;

        [SetUp]
        public void SetUp()
        {
            player = new Player { Id = 1234, X = playerStartX, Y = playerStartY, IsIt = true };
            allPlayers = new List<Player> { player };
            GetMock<IMapSettings>().Setup(x => x.MapWidth).Returns(mapWidth);
            GetMock<IMapSettings>().Setup(x => x.MapHeight).Returns(mapHeight);
            GetMock<IPlayerRepository>().Setup(x => x.GetAllPlayers()).Returns(allPlayers);
        }

        [Test]
        public void It_should_return_the_player_position()
        {
            result = ClassUnderTest.Execute(1234);

            Assert.That(result.X, Is.EqualTo(23));
            Assert.That(result.Y, Is.EqualTo(12));
            Assert.That(result.IsIt, Is.EqualTo(true));
        }
    }
}