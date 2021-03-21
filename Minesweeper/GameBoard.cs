using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

		public GameBoard(int width, int height)
		{
			Width = width;
			Height = height;

			Board = new Cell[width, height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Board[x, y] = new Cell() {CellState = CellState.Empty};
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
				var x = random.Next(0, Width - 1);
				var y = random.Next(0, Height - 1);

				if (Board[x, y].CellState == CellState.Mine) continue;

				Board[x, y].CellState = CellState.Mine;
				
				mineCount--;

				int[,] directions = {{0, -1}, {+1, -1}, {+1, 0}, {+1, +1}, {0, +1}, {-1, +1}, {-1, 0}, {-1, -1}};

				for (var i = 0; i < 8; i++)
				{
					int xDir = directions[i, 0];
					int yDir = directions[i, 1];
					

					if (x + xDir < 0 || y + yDir < 0 || x + xDir > Width - 1 || y + yDir > Height - 1)
					{
						continue;
					}
					if (Board[x + xDir, y + yDir].CellState == CellState.MinesAround || 
					    Board[x + xDir, y + yDir].CellState == CellState.Empty)
					{
						Board[x + xDir, y + yDir].CellState = CellState.MinesAround;
						Board[x + xDir, y + yDir].MinesAround++;
					}

				}
				
			}
			
			printBoard();
		}

		private void printBoard()
		{
			for (var y = 0; y < Height; y++)
			{
				for (var x = 0; x < Width; x++)
				{
					if (Board[x, y].CellState == CellState.Mine)
					{
						Debug.Write("x");	
					}
					else
					{
						Debug.Write($"{Board[x, y].MinesAround}");	
					}
					
				}
				Debug.WriteLine("");
			}
		}
	}
}