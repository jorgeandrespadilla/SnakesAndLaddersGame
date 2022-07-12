namespace SnakesAndLadders.Models
{
    public class Player
    {
        public readonly string Id;

        public readonly string Name;

        public Player(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }
    }
}
