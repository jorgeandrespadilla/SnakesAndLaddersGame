using SnakesAndLadders.Common;

namespace SnakesAndLadders.Models
{
    public class Board
    {
        public const string InvalidBoardSize = "Invalid board size";
        public const string InvalidBoardAdornments = "Invalid board adornments";
        public const string EmptyPlayerName = "Player names can't be empty";
        public const string RepeatedPlayerName = "Player names can't be repeated";

        public const int InitialPlayerPosition = 1;
        public const int MinBoardSize = 2;

        /// <summary>
        /// Board size (number of cells)
        /// </summary>
        private readonly int _size;
        public int Size => _size;

        private readonly IList<BoardAdornment> _adornments;

        private readonly IList<Player> _players = new List<Player>();

        private readonly Dictionary<string, int> _playersPosition = new();


        public Board(int size, IList<BoardAdornment> adornments)
        {
            AssertBoardSizeIsValid(size);
            AssertBoardAdornmentsAreValid(adornments);
            _size = size;
            _adornments = adornments;
        }

        public void AddPlayer(Player player)
        {
            AssertPlayerIsValid(player);
            _players.Add(player);
            _playersPosition.Add(player.Id, InitialPlayerPosition);
        }

        public int PlayersCount()
        {
            return _players.Count;
        }

        public int GetPositionOfPlayer(Player player)
        {
            return _playersPosition[player.Id];
        }

        public void SetPositionOfPlayer(Player player, int position)
        {
            _playersPosition[player.Id] = position;
        }

        public BoardAdornment? GetBoardAdornmentAtPosition(int position)
        {
            return _adornments.FirstOrDefault(adornment => adornment.StartsOn(position));
        }

        public Player? GetWinner()
        {
            return _players.FirstOrDefault(player => GetPositionOfPlayer(player) == Size);
        }


        #region Validation and Assertion Methods

        private void AssertBoardSizeIsValid(int size)
        {
            if (size < MinBoardSize)
                throw new GameException(InvalidBoardSize);
        }

        private void AssertBoardAdornmentsAreValid(IList<BoardAdornment> newAdornments)
        {
            var validBoardAdornments = new List<BoardAdornment>();
            foreach (var newAdornment in newAdornments)
            {
                if (!IsBoardAdornmentValid(newAdornment, validBoardAdornments))
                    throw new GameException(InvalidBoardAdornments);
                validBoardAdornments.Add(newAdornment);
            }
        }

        private bool IsBoardAdornmentValid(BoardAdornment newAdornment, IList<BoardAdornment> validAdornments)
        {
            if (newAdornment.StartsOn(newAdornment.End))
                return false;
            // New adornments can't start in the last board position
            if (newAdornment.StartsOn(Size))
                return false;
            foreach (var adornment in validAdornments)
            {
                // New adornments can't be similar to an existing one
                if (adornment.Start == newAdornment.Start && adornment.End == newAdornment.End)
                    return false;
                // New adornments can't be the opposite to an existing one
                if (adornment.Start == newAdornment.End && adornment.End == newAdornment.Start)
                    return false;
            }
            return true;
        }

        private void AssertPlayerIsValid(Player player)
        {
            if (string.IsNullOrEmpty(player.Name))
                throw new GameException(EmptyPlayerName);
            var hasRepeatedPlayerName = _players.FirstOrDefault(p => p.Name == player.Name);
            if (hasRepeatedPlayerName != null)
                throw new GameException(RepeatedPlayerName);
        }

        #endregion
    }
}
