namespace SnakesAndLadders.Models
{
    public class GameSettings
    {
        /// <summary>
        /// Minimum amount of players required
        /// </summary>
        public int MinPlayersAmount { get; set; }
        /// <summary>
        /// Maximum amount of players allowed
        /// </summary>
        public int MaxPlayersAmount { get; set; }
        /// <summary>
        /// Maximum amount of movements throught board adornments
        /// This allows to avoid infinite loops due to bad adornment configuration
        /// </summary>
        public int MaxMovesThroughBoardAdornments { get; set; }
    }
}
