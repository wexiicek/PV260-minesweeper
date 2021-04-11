using System;
using Math = System.Math;

namespace PV260_Minesweeper
{
	public class GameBoard
	{
		private readonly Random random = new Random();
		public int Width { get; set; }
		public int Height { get; set; }
		public Cell[,] Board { get; set; }

		public int RemainingMineCount { get; set; }

		public GameBoard(int width, int height)
		{
			Width = width;
			Height = height;

			Board = new Cell[width, height];

			for (int row = 0; row < width; row++)
			{
				for (int col = 0; col < height; col++)
				{
					Board[row, col] = new Cell() {State = State.Empty, Display = Display.Hidden};
				}
			}
		}

		public void AddMinesToTheBoard()
		{
			var min = (int) Math.Ceiling(Width * Height * 0.2);
			var max = (int) Math.Floor(Width * Height * 0.6);
			var mineCount = random.Next(min, max);
			RemainingMineCount = mineCount;

			while (mineCount > 0)
			{
				var row = random.Next(0, Width - 1);
				var column = random.Next(0, Height - 1);

				if (Board[row, column].State == State.Mine) continue;

				Board[row, column].State = State.Mine;
				mineCount--;

				for (var i = 0; i < 8; i++)
				{
					int xDir = Helper.Directions[i, 0];
					int yDir = Helper.Directions[i, 1];

					if (Helper.CellIsOutOfTheBoardRange(xDir, yDir, row, column, Width, Height)) continue;

					if (Board[row + yDir, column + xDir].State == State.MinesAround || 
					    Board[row + yDir, column + xDir].State == State.Empty)
					{
						Board[row + yDir, column + xDir].State = State.MinesAround;
						Board[row + yDir, column + xDir].MinesAround++;
					}
				}
			}
		}

		public void ExposeAllEmptyCellsAround(int row, int column)
		{
			Board[row, column].Display = Display.Visible;

			if (Board[row, column].State != State.Empty) return;

			for (var i = 0; i < 8; i++)
			{
				int xDir = Helper.Directions[i, 0];
				int yDir = Helper.Directions[i, 1];

				if (Helper.CellIsOutOfTheBoardRange(xDir, yDir, row, column, Width, Height)) continue;

				if (Board[row + yDir, column + xDir].Display != Display.Visible)
				{
					ExposeAllEmptyCellsAround(row + yDir, column + xDir);
				}
			}
		}
	}
}