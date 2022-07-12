using SnakesAndLadders.Interfaces;
using System.Collections.Generic;

namespace SnakesAndLaddersTests.Common
{
    public class MockDiceService : IDiceService
    {
        private readonly IList<int> _rolls;
        private int _currentRollIndex = 0;

        public MockDiceService(List<int> rolls)
        {
            _rolls = rolls;
        }

        public int Roll()
        {
            var currentRoll = _rolls[_currentRollIndex];
            CalculateNextRoll();
            return currentRoll;
        }

        private void CalculateNextRoll()
        {
            _currentRollIndex = (_currentRollIndex + 1) % _rolls.Count;
        }
    }
}
