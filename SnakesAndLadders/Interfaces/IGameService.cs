using SnakesAndLadders.Models;

namespace SnakesAndLadders.Interfaces
{
    public interface IGameService
    {
        void Start();
        PlayerMove NextMove();
        void AddPlayer(Player player);
        Player GetCurrentPlayer();
        Player? Winner();
        bool IsOver();
    }
}
