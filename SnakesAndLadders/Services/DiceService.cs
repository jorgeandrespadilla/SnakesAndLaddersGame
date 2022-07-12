using SnakesAndLadders.Interfaces;

namespace SnakesAndLadders.Services
{
    public class DiceService: IDiceService
    {
        private readonly int MinValue;
        private readonly int MaxValue;

        public DiceService(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public int Roll()
        {
            Random random = new();
            return random.Next(MinValue, MaxValue + 1);
        }
    }
}
