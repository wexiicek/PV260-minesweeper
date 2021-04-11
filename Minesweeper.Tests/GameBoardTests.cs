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
		[TestCase(20, 5)]
		public void AddMinesToTheBoard_MinesCountInRange(int width, int height)
		{
			var board = new GameBoard(width, height);
			board.AddMinesToTheBoard();

			var min = Math.Ceiling(width * height * 0.2);
			var max = Math.Floor(width * height * 0.6);

			Assert.GreaterOrEqual(board.RemainingMineCount, min);
			Assert.LessOrEqual(board.RemainingMineCount, max);
		}

		[TestCase(10, 10)]
		[TestCase(20, 5)]
		public void AddMinesToTheBoard_CorrectCountOfMinesIsPlanted(int width, int height)
		{
			var board = new GameBoard(width, height);
			board.AddMinesToTheBoard();

			var numberOfMines = board.Board.Cast<Cell>().Count(cell => cell.State == State.Mine);

			Assert.AreEqual(board.RemainingMineCount, numberOfMines);
		}

		[TestCase(10, 10)]
		[TestCase(5, 50)]
		[TestCase(50, 5)]
		[TestCase(50, 50)]
		public void AddMinesToTheBoard_CorrectNumberOfMinesAroundEveryCell(int width, int height)
		{
			var gameBoard = new GameBoard(width, height);
			gameBoard.AddMinesToTheBoard();
			var board = gameBoard.Board;

			for (int row = 0; row < width; row++)
			{
				for (int column = 0; column < height; column++)
				{
					var numberOfMinesAround = 0;

					if (board[row, column].State != State.MinesAround) continue;

					var actualCellMinesAround = board[row, column].MinesAround;

					for (var i = 0; i < 8; i++)
					{
						int xDir = Helper.Directions[i, 0];
						int yDir = Helper.Directions[i, 1];

						if (Helper.CellIsOutOfTheBoardRange(xDir, yDir, row, column, width, height)) continue;

						if (board[row + yDir, column + xDir].State == State.Mine)
						{
							numberOfMinesAround++;
						}
					}

					Assert.AreEqual(actualCellMinesAround, numberOfMinesAround);
				}
			}
		}

		[Test]
		public void ExposeAllEmptyCellsAround_MineInTheCorner_EmptyCellsAroundAreExposedWithFirstMinesAroundCell()
		{
			var gameBoard = new GameBoard(4, 4);
			var board = gameBoard.Board;

			board[0, 0].State = State.Mine;

			board[0, 1].State = State.MinesAround;
			board[1, 0].State = State.MinesAround;
			board[1, 1].State = State.MinesAround;

			var hidden = Display.Hidden;
			var visible = Display.Visible;

			var expected = new[]
			{
				hidden, visible, visible, visible,
				visible, visible, visible, visible,
				visible, visible, visible, visible,
				visible, visible, visible, visible
			};

			gameBoard.ExposeAllEmptyCellsAround(2, 2);

			Assert.AreEqual(expected, board.Cast<Cell>().Select(x => x.Display).ToArray());
		}

		[Test]
		public void ExposeAllEmptyCellsAround_MineInTheMiddle_EmptyCellsAroundAreExposedWithFirstMinesAroundCell()
		{
			var gameBoard = new GameBoard(4, 4);
			var board = gameBoard.Board;

			board[1, 1].State = State.Mine;

			board[0, 0].State = State.MinesAround;
			board[0, 1].State = State.MinesAround;
			board[0, 2].State = State.MinesAround;
			board[1, 0].State = State.MinesAround;
			board[1, 2].State = State.MinesAround;
			board[2, 0].State = State.MinesAround;
			board[2, 1].State = State.MinesAround;
			board[2, 2].State = State.MinesAround;

			var hidden = Display.Hidden;
			var visible = Display.Visible;

			var expected = new[]
			{
				hidden, hidden, visible, visible,
				hidden, hidden, visible, visible,
				visible, visible, visible, visible,
				visible, visible, visible, visible
			};

			gameBoard.ExposeAllEmptyCellsAround(0, 3);

			Assert.AreEqual(expected, board.Cast<Cell>().Select(x => x.Display).ToArray());
		}

		[Test]
		public void ExposeAllEmptyCellsAround_TwoMines_EmptyCellsAroundAreExposedWithFirstMinesAroundCell()
		{
			var gameBoard = new GameBoard(4, 4);
			var board = gameBoard.Board;

			board[0, 1].State = State.Mine;
			board[3, 3].State = State.Mine;

			board[0, 0].State = State.MinesAround;
			board[1, 0].State = State.MinesAround;
			board[1, 1].State = State.MinesAround;
			board[0, 2].State = State.MinesAround;
			board[1, 2].State = State.MinesAround;
			board[2, 2].State = State.MinesAround;
			board[3, 2].State = State.MinesAround;
			board[2, 3].State = State.MinesAround;

			var hidden = Display.Hidden;
			var visible = Display.Visible;

			var firstExpected = new[]
			{
				hidden, hidden, visible, visible,
				hidden, hidden, visible, visible,
				hidden, hidden, visible, visible,
				hidden, hidden, hidden, hidden
			};

			var secondExpected = new[]
			{
				hidden, hidden, visible, visible,
				visible, visible, visible, visible,
				visible, visible, visible, visible,
				visible, visible, visible, hidden
			};

			gameBoard.ExposeAllEmptyCellsAround(0, 3);
			Assert.AreEqual(firstExpected, board.Cast<Cell>().Select(x => x.Display).ToArray());

			gameBoard.ExposeAllEmptyCellsAround(3, 1);
			Assert.AreEqual(secondExpected, board.Cast<Cell>().Select(x => x.Display).ToArray());
		}
	}
}
