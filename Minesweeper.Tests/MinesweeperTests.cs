using System;
using NUnit.Framework;
using PV260_Minesweeper;

namespace MinesweeperTests
{
    public class Tests
    {
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(50, 51)]
        [TestCase(51, 50)]
        [TestCase(51, 51)]
        public void GenerateGameBoard_InvalidSize_ThrowsException(int width, int height)
        {
	        Assert.Throws<ArgumentOutOfRangeException>(() => Minesweeper.GenerateGameBoard(width, height));
        }

        [TestCase(3, 3)]
        [TestCase(25, 25)]
        [TestCase(50, 50)]
        public void GenerateGameBoard_ValidSize_ReturnsBoard(int width, int height)
        {
	        var board = Minesweeper.GenerateGameBoard(width, height);

            Assert.IsInstanceOf<GameBoard>(board);
        }
    }
}