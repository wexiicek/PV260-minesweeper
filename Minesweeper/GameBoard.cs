using System;
using System.Diagnostics;
using Math = System.Math;

namespace PV260_Minesweeper
{
	public class GameBoard
	{
		private readonly Random random = new Random();
		public int Width { get; set; }
		public int Height { get; set; }
		public Cell[,] Board { get; set; }

		public int MineCount { get; set; }

		private int[,] directions = { { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 } };

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
			MineCount = mineCount;

			while (mineCount > 0)
			{
				var row = random.Next(0, Width - 1);
				var column = random.Next(0, Height - 1);

				if (Board[row, column].State == State.Mine) continue;

				Board[row, column].State = State.Mine;
				mineCount--;

				for (var i = 0; i < 8; i++)
				{
					int xDir = directions[i, 0];
					int yDir = directions[i, 1];

					if (row + yDir < 0 || column + xDir < 0 || row + yDir > Width - 1 || column + xDir > Height - 1) continue;

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
				int xDir = directions[i, 0];
				int yDir = directions[i, 1];

				if (row + yDir < 0 || column + xDir < 0 || row + yDir > Width - 1 || column + xDir > Height - 1)
				{
					continue;
				}

				if (Board[row + yDir, column + xDir].Display != Display.Visible)
				{
					ExposeAllEmptyCellsAround(row + yDir, column + xDir);
				}
			}
		}

		public void printBoard()
		{
			Debug.WriteLine("===============================================");
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					if (Board[x, y].Display == Display.Visible && Board[x, y].State == State.Empty)
					{
						Debug.Write("#");
						continue;
					}

					if (Board[x, y].State == State.Mine)
					{
						Debug.Write("row");	
					}
					else
					{
						Debug.Write($"{Board[x, y].MinesAround}");	
					}
					
				}
				Debug.WriteLine(" ");
			}
			Debug.WriteLine("===============================================");
		}
	}
}