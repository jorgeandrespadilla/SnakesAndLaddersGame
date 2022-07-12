using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakesAndLadders.Models;
using SnakesAndLadders.Services;
using SnakesAndLaddersTests.Common;
using System.Collections.Generic;
using System.Reflection;

namespace SnakesAndLaddersTests
{
    [TestClass]
    public class PlayerTests
    {
        private Player _player;

        [TestInitialize]
        public void SetUp()
        {
            _player = new Player("Player");
        }

        [TestMethod]
        public void Player_WhenPlacedOnBoard_ShouldStartAtPosition1()
        {
            var board = TestObjects.BoardWithoutAndornments();
            var dice = new MockDiceService(new List<int>());
            var settings = TestObjects.GameSettings();

            var game = new GameService(board, dice, settings);

            game.AddPlayer(_player);

            Assert.AreEqual(board.GetPositionOfPlayer(_player), Board.InitialPlayerPosition);
        }

        [TestMethod]
        public void Player_WhenMoving3SpacesFromPosition1_ShouldEndAtPosition4()
        {
            var board = TestObjects.BoardWithoutAndornments();
            var dice = new MockDiceService(new List<int> { 3 });
            var settings = TestObjects.GameSettings();

            var game = new GameService(board, dice, settings);

            game.AddPlayer(_player);
            game.Start();
            game.NextMove();

            int expectedPosition = 4;
            Assert.AreEqual(board.GetPositionOfPlayer(_player), expectedPosition);
        }

        [TestMethod]
        public void Player_WhenMoving3SpacesAndThen4SpacesFromPosition1_ShouldEndAtPosition8()
        {
            var board = TestObjects.BoardWithoutAndornments();
            var dice = new MockDiceService(new List<int> { 3, 4 });
            var settings = TestObjects.GameSettings();

            var game = new GameService(board, dice, settings);

            game.AddPlayer(_player);
            game.Start();
            game.NextMove();
            game.NextMove();

            int expectedPosition = 8;
            Assert.AreEqual(board.GetPositionOfPlayer(_player), expectedPosition);
        }

        [TestMethod]
        public void Player_WhenMoving3FromPosition97_ShouldEndAtPosition100AndWinTheGame()
        {
            var board = TestObjects.BoardWithoutAndornments();
            var currentPosition = 97 - Board.InitialPlayerPosition;
            var dice = new MockDiceService(new List<int> { currentPosition, 3 });
            var settings = TestObjects.GameSettings();

            var game = new GameService(board, dice, settings);

            game.AddPlayer(_player);
            game.Start();
            game.NextMove();
            game.NextMove();

            int expectedPosition = 100;
            Assert.AreEqual(board.GetPositionOfPlayer(_player), expectedPosition);
            Assert.AreSame(game.Winner(), _player);
        }

        [TestMethod]
        public void Player_WhenMoving4FromPosition97_ShouldEndAtPosition97AndNotWinTheGame()
        {
            var board = TestObjects.BoardWithoutAndornments();
            var currentPosition = 97 - Board.InitialPlayerPosition;
            var dice = new MockDiceService(new List<int> { currentPosition, 4 });
            var settings = TestObjects.GameSettings();

            var game = new GameService(board, dice, settings);

            game.AddPlayer(_player);
            game.Start();
            game.NextMove();
            game.NextMove();

            int expectedPosition = 97;
            Assert.AreEqual(board.GetPositionOfPlayer(_player), expectedPosition);
            Assert.AreNotSame(game.Winner(), _player);
        }
    }
}
