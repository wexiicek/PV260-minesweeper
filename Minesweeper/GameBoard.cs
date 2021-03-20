using System.Collections.Generic;

namespace PV260_Minesweeper
{
    public class GameBoard
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Cell[,] Board { get; set; }

        public GameBoard()
        {
            
        }
    }
}