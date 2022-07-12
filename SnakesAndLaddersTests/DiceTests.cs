using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnakesAndLaddersTests.Common;
using System.Collections.Generic;

namespace SnakesAndLaddersTests
{
    [TestClass]
    public class DiceTests
    {
        [TestMethod]
        public void Dice_WhenRolled_ShouldBeBetween1And6()
        {
            var dice = new MockDiceService(new List<int> { 1, 2, 3, 4, 5, 6 });
            var result = dice.Roll();
            Assert.IsTrue(result >= 1 && result <= 6);
        }
    }
}
