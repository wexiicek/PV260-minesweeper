using System;

namespace PV260_Minesweeper
{
    public static class Minesweeper
    {
		public static GameStatus GameStatus { get; private set; }
		public static GameBoard GameBoard { get; private set; }

		private static int _flaggedCount;
		private static int _initialMinesCount;

		public static GameBoard GenerateGameBoard(int width, int height)
        {
	        if (width < 3 || width > 50 || height < 3 || height > 50)
	        {
		        throw new ArgumentOutOfRangeException($"Height {height} or width {width} is out of valid size");
	        }

	        GameStatus = GameStatus.InProgress;
	        GameBoard = new GameBoard(width, height);
	        GameBoard.AddMinesToTheBoard();

	        _initialMinesCount = GameBoard.MineCount;
	        _flaggedCount = 0;

	        return GameBoard;
        }

        public static GameBoard Uncover(int row, int column)
        {
	        return GameBoard;

        }

        public static GameBoard Flag(int row, int column)
        {
	        return GameBoard;
		}
    }
}