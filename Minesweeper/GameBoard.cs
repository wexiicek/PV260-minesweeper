using System;
using System.Collections.Generic;

namespace PV260_Minesweeper
{
    public class GameBoard
    {
        private Random random;   
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell[,] Board { get; set; }
        
        public int MineCount { get; set; }

        public GameBoard(int width, int height)
        {
            Width = width;
            Height = height;

            Board = new Cell[width, height];
        }
    }
}