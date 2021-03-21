using System;
using PV260_Minesweeper;

namespace PV260_Minesweeper
{
    public static class Minesweeper
    {
		private static GameBoard gameBoard;

		public static GameBoard GenerateGameBoard(int width, int height)
        {
	        if (width < 3 || width > 50 || height < 3 || height > 50)
	        {
		        throw new ArgumentOutOfRangeException($"Height {height} or width {width} is out of valid size");
	        }

	        return new GameBoard(width, height);
        }

        public static void Play()
        {
            
        }
    }
}