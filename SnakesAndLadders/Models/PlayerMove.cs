namespace SnakesAndLadders.Models
{
    /// <summary>
    /// Player movement information
    /// </summary>
    public class PlayerMove
    {
        /// <summary>
        /// Player referencc
        /// </summary>
        public Player Player { get; set; }
        
        /// <summary>
        /// Previous position of the player on the board
        /// </summary>
        public int PreviousPosition { get; set; }

        /// <summary>
        /// Position of the player on the board after dice roll (before moving through board adornments)
        /// </summary>
        public int PositionAfterDiceRoll { get; set; }
        
        /// <summary>
        /// Position of the player on the board after moving through board adornments (final position)
        /// </summary>
        public int PositionAfterMovingThroughBoardAdornments { get; set; }

        /// <summary>
        /// Value obtained by rolling the dice
        /// </summary>
        public int RolledNumber { get; set; }

        /// <summary>
        /// List of game adornments used by the player
        /// </summary>
        public IList<BoardAdornment> BoardAdornments { get; set; }

        public PlayerMove(Player player)
        {
            Player = player;
            BoardAdornments = new List<BoardAdornment>();
        }
    }
}
