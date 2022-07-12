using SnakesAndLadders.Models;
using System.Collections.Generic;

namespace SnakesAndLaddersTests.Common
{
    public static class TestObjects
    {
        public static Board BoardWithoutAndornments()
        {
            return new Board(100, new List<BoardAdornment>());
        }

        public static GameSettings GameSettings()
        {
            return new GameSettings
            {
                MinPlayersAmount = 1,
                MaxPlayersAmount = 10,
                MaxMovesThroughBoardAdornments = 5
            };
        }
    }
}
