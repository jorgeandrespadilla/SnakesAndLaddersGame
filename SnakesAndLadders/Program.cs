using SnakesAndLadders.Common;
using SnakesAndLadders.Common.Enums;
using SnakesAndLadders.Interfaces;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;

namespace SnakesAndLadders
{
    public class Program
    {

        static void Main(string[] args)
        {
            

            IGameService game;
            try
            {
                game = ConfigureGame();
            }
            catch (GameException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("An unexpected error ocurred while configuring the game");
                return;
            }

            bool exit = false;
            do
            {
                ShowMenu();
                var option = GetMenuOption(1, 3);

                switch (option)
                {
                    case 1:
                        AddPlayerToGame(game);
                        break;
                    case 2:
                        try
                        {
                            StartGame(game);                           
                            exit = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case 3:
                        exit = true;
                        break;
                    default:
                        break;
                }

                Console.Clear();
            }
            while (!exit);
        }

        private static IGameService ConfigureGame()
        {
            var board = new Board(100, GenerateBoardAdornments());
            var dice = new DiceService(1, 6);
            var settings = new GameSettings
            {
                MinPlayersAmount = 2,
                MaxPlayersAmount = 100,
                MaxMovesThroughBoardAdornments = 5
            };
            return new GameService(board, dice, settings);
        }

        private static IList<BoardAdornment> GenerateBoardAdornments()
        {
            var adornments = new List<BoardAdornment>
            {
                new (16, 6, BoardAdornmentType.Snake),
                new (49, 11, BoardAdornmentType.Snake),
                new (46, 25, BoardAdornmentType.Snake),
                new (62, 19, BoardAdornmentType.Snake),
                new (64, 60, BoardAdornmentType.Snake),
                new (74, 53, BoardAdornmentType.Snake),
                new (89, 68, BoardAdornmentType.Snake),
                new (92, 88, BoardAdornmentType.Snake),
                new (95, 75, BoardAdornmentType.Snake),
                new (99, 80, BoardAdornmentType.Snake),
                new (2, 38, BoardAdornmentType.Ladder),
                new (7, 14, BoardAdornmentType.Ladder),
                new (8, 31, BoardAdornmentType.Ladder),
                new (15, 26, BoardAdornmentType.Ladder),
                new (21, 42, BoardAdornmentType.Ladder),
                new (28, 84, BoardAdornmentType.Ladder),
                new (36, 44, BoardAdornmentType.Ladder),
                new (51, 67, BoardAdornmentType.Ladder),
                new (71, 91, BoardAdornmentType.Ladder),
                new (78, 98, BoardAdornmentType.Ladder),
                new (87, 94, BoardAdornmentType.Ladder)
            };
            return adornments;
        }

        private static void ShowMenu()
        {
            Console.WriteLine("### SNAKES & LADDERS ###");
            Console.WriteLine();

            Console.WriteLine("1. Add player");
            Console.WriteLine("2. Start game");
            Console.WriteLine("3. EXIT");
        }

        private static int GetMenuOption(int minValue, int maxValue)
        {
            int option;
            bool validOption;
            do
            {
                Console.Write("> ");
                bool validInput = int.TryParse(Console.ReadLine(), out option);
                bool validNumber = option >= minValue && option <= maxValue;
                validOption = validInput && validNumber;
                if (!validOption)
                    Console.WriteLine("Invalid option");
            }
            while (!validOption);
            return option;
        }

        private static void AddPlayerToGame(IGameService game)
        {
            bool validPlayer;
            do
            {
                try
                {
                    Console.Write("Player name: ");
                    string name = Console.ReadLine() ?? "";
                    game.AddPlayer(new Player(name));
                    validPlayer = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    validPlayer = false;
                }
            }
            while (!validPlayer);            
        }

        private static void StartGame(IGameService game)
        {
            game.Start();
            Console.WriteLine();
            while (!game.IsOver())
            {
                var currentPlayer = game.GetCurrentPlayer();
                Console.WriteLine($"{currentPlayer.Name} it's your turn (press ENTER)");
                Console.ReadLine();

                var currentMove = game.NextMove();
                if (currentMove.PreviousPosition == currentMove.PositionAfterDiceRoll)
                    Console.WriteLine($"{currentMove.Player.Name} gets a {currentMove.RolledNumber} and stays in position {currentMove.PositionAfterDiceRoll}");
                else
                    Console.WriteLine($"{currentMove.Player.Name} gets a {currentMove.RolledNumber} and moves from {currentMove.PreviousPosition} to {currentMove.PositionAfterDiceRoll}");
                
                foreach (var adornment in currentMove.BoardAdornments)
                {
                    Console.WriteLine($"{currentMove.Player.Name} reaches a {adornment.GetTypeDescription()} in position {adornment.Start} and moves to position {adornment.End}");
                }
                Console.WriteLine($"Final position of {currentMove.Player.Name} is {currentMove.PositionAfterMovingThroughBoardAdornments}");
                Console.WriteLine();
            }
            Console.WriteLine($"{game.Winner()!.Name} has won the game!");
            Console.WriteLine();
        }
    }
}