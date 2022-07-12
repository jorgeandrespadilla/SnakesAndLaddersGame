using SnakesAndLadders.Common;
using SnakesAndLadders.Common.Enums;
using SnakesAndLadders.Interfaces;
using SnakesAndLadders.Models;

namespace SnakesAndLadders.Services
{
    public class GameService : IGameService
    {
        public const string AlreadyStartedGame = "Game has already started";
        public const string GameNotInProgress = "Game is not currently in progress";
        public const string InvalidPlayersAmount = "Game needs between {0} and {1} players before starting";
        public const string PlayersAmountExceeded = "Game can only have a maximum of {0} players";


        private readonly Board _board;

        private readonly IDiceService _dice;
        
        private readonly GameSettings _settings;
        
        private readonly Queue<Player> _playersQueue = new();

        private GameStatus _status;

        
        public GameService(Board board, IDiceService dice, GameSettings settings)
        {
            _board = board;
            _dice = dice;
            _settings = settings;
            _status = GameStatus.NotStarted;
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            AssertGameCanStart();
            _status = GameStatus.InProgress;
        }

        /// <summary>
        /// Runs the next player move in the game.
        /// </summary
        /// <returns>
        /// The player movement information.
        /// </returns>
        public PlayerMove NextMove()
        {
            AssertGameIsInProgress();
            Player currentPlayer = GetCurrentPlayer();
            int numberOfCells = _dice.Roll();
            var playerMove = MovePlayer(currentPlayer, numberOfCells);
            CalculatePlayerStateInQueue(playerMove);
            CalculateGameStatus();
            return playerMove;
        }

        private void CalculateGameStatus()
        {
            if (Winner() != null)
                _status = GameStatus.Finished;
        }

        /// <summary>
        /// Adds a given player to the game.
        /// </summary>
        public void AddPlayer(Player player)
        {
            AssertPlayersAmountNotExceeded();
            _board.AddPlayer(player);
            _playersQueue.Enqueue(player);
        }

        /// <summary>
        /// Gets the current player to be moved.
        /// </summary>
        /// <returns>
        /// The current player reference.
        /// </returns>
        public Player GetCurrentPlayer()
        {
            return _playersQueue.Peek();
        }

        public Player? Winner()
        {
            return _board.GetWinner();
        }

        public bool IsOver()
        {
            return _status == GameStatus.Finished;
        }
        

        /// <summary>
        /// Moves a given player through the board, given a given a number of positions to be moved.
        /// </summary>
        /// <returns>
        /// The player movement information.
        /// </returns>
        private PlayerMove MovePlayer(Player player, int numberOfCells)
        {
            var playerMove = new PlayerMove(player)
            {
                PreviousPosition = _board.GetPositionOfPlayer(player),
                RolledNumber = numberOfCells
            };

            int newPosition = _board.GetPositionOfPlayer(player) + numberOfCells;
            if (newPosition > _board.Size)
                newPosition = _board.GetPositionOfPlayer(player);
            
            _board.SetPositionOfPlayer(player, newPosition);
            playerMove.PositionAfterDiceRoll = newPosition;
            
            var crossedBoardAdornments = MoveThroughBoardAdornments(player);
            playerMove.PositionAfterMovingThroughBoardAdornments = _board.GetPositionOfPlayer(player);
            playerMove.BoardAdornments = crossedBoardAdornments;

            return playerMove;
        }

        /// <summary>
        /// Moves a given player through the board by taking into consideration any board adornment.
        /// </summary>
        /// <returns>
        /// A list of board adornments used by the player. 
        /// If no board adornments are used by the player, returns a empty list.
        /// </returns>
        private IList<BoardAdornment> MoveThroughBoardAdornments(Player player)
        {
            IList<BoardAdornment> crossedBoardAdornments = new List<BoardAdornment>();
            bool hasPlayerMoved = true;
            int movesCount = 0;
            // Move through board adornments while not exceeding a maximum amount of moves in a single dice roll
            while (hasPlayerMoved && movesCount < _settings.MaxMovesThroughBoardAdornments)
            {
                hasPlayerMoved = false;
                var adornment = _board.GetBoardAdornmentAtPosition(_board.GetPositionOfPlayer(player));
                if (adornment != null)
                {
                    crossedBoardAdornments.Add(adornment);
                    _board.SetPositionOfPlayer(player, adornment.End);
                    hasPlayerMoved = true;
                }
                movesCount++;
            }
            return crossedBoardAdornments;
        }

        /// <summary>
        /// Determines player state in queue (enqueued or kept in its current position)
        /// based on its move and game information.
        /// 
        /// Currently, player movements are handled consecutively.
        /// For example, if the game requires defining a custom logic for non-consecutive movements, 
        /// specific conditions could be defined within this method.
        /// </summary>
        private void CalculatePlayerStateInQueue(PlayerMove playerMove)
        {
            _ = _playersQueue.Dequeue();
            _playersQueue.Enqueue(playerMove.Player);
        }



        #region Validation And Assertion Methods

        /// <summary>
        /// Validates game before starting
        /// </summary>
        private void AssertGameCanStart()
        {
            if (_status != GameStatus.NotStarted)
                throw new GameException(AlreadyStartedGame);

            if (_board.PlayersCount() < _settings.MinPlayersAmount ||
                _board.PlayersCount() > _settings.MaxPlayersAmount)
                throw new GameException(string.Format(InvalidPlayersAmount, _settings.MinPlayersAmount, _settings.MaxPlayersAmount));
        }

        /// <summary>
        /// Validates maximum players amount.
        /// </summary>
        private void AssertPlayersAmountNotExceeded()
        {
            var playersAmountExceeded = (_board.PlayersCount() + 1) > _settings.MaxPlayersAmount;
            if (playersAmountExceeded)
                throw new GameException(string.Format(PlayersAmountExceeded, _settings.MaxPlayersAmount));
        }

        /// <summary>
        /// Validates if game is in progress.
        /// </summary>
        private void AssertGameIsInProgress()
        {
            if (_status != GameStatus.InProgress)
                throw new GameException(GameNotInProgress);
        } 

        #endregion
    }
}
