using System;
using System.Linq;
using NUnit.Framework;
using PV260_Minesweeper;

namespace MinesweeperTests
{
	[TestFixture]
	public class GameBoardTests
	{
		[TestCase(10, 10)]
		public void AddMinesToTheBoard_MinesCountInRange(int width, int height)
		{
			var board = new GameBoard(width, height);
			board.AddMinesToTheBoard();

			var min = Math.Ceiling(width * height * 0.2);
			var max = Math.Floor(width * height * 0.6);

			Assert.GreaterOrEqual(board.MineCount, min);
			Assert.LessOrEqual(board.MineCount, max);
		}

		[TestCase(10, 10)]
		public void AddMinesToTheBoard_CorrectCountOfMinesIsPlanted(int width, int height)
		{
			var board = new GameBoard(width, height);
			board.AddMinesToTheBoard();

			var numberOfMines = board.Board.Cast<Cell>().Count(cell => cell.CellState == CellState.Mine);

			Assert.AreEqual(board.MineCount, numberOfMines);
		} 
    }
}
