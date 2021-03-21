using System;
using System.Collections.Generic;
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
            var min = (int)Math.Ceiling(Width * Height * 0.2);
            var max = (int)Math.Floor(Width * Height * 0.6);
            var mineCount = random.Next(min, max);
            MineCount = mineCount;

            while (mineCount > 0)
            {
	            var x = random.Next(0, Width - 1);
	            var y = random.Next(0, Height - 1);

	            if (Board[x, y].CellState != CellState.Empty) continue;

	            Board[x, y].CellState = CellState.Mine;
	            mineCount--;
            }
        }
    }
}