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
	        if (Helper.UncorrectSizeOfTheBoard(width, height))
	        {
		        throw new ArgumentOutOfRangeException($"Height {height} or width {width} is out of valid size");
	        }

	        GameBoard = new GameBoard(width, height);
	        GameBoard.AddMinesToTheBoard();
	        GameStatus = GameStatus.InProgress;

	        _initialMinesCount = GameBoard.RemainingMineCount;
	        _flaggedCount = 0;

	        return GameBoard;
        }

		public static void Reset()
		{
			GameStatus = GameStatus.NotStarted;
			GameBoard = null;
			_flaggedCount = 0;
			_initialMinesCount = 0;
		}

        public static GameBoard Uncover(int row, int column)
        {
	        Validate(row, column);

	        var cell = GameBoard.Board[row, column];

	        if (cell.Display == Display.Visible ||
	            cell.Display == Display.Flag) return GameBoard;

	        switch (cell.State)
	        {
		        case State.Empty:
			        GameBoard.ExposeAllEmptyCellsAround(row, column);
			        break;
		        case State.Mine:
			        cell.Display = Display.Visible;
			        GameStatus = GameStatus.GameOver;
			        break;
		        case State.MinesAround:
			        cell.Display = Display.Visible;
			        break;
	        }

	        return GameBoard;
        }

        public static GameBoard Flag(int row, int column)
        {
	        Validate(row, column);

	        var cell = GameBoard.Board[row, column];

	        return cell.Display switch
	        {
		        Display.Visible => GameBoard,
		        Display.Flag => FlagOnAlreadyFlaggedCell(cell),
		        _ => FlagOnHiddenCell(cell)
	        };
        }

        private static GameBoard FlagOnAlreadyFlaggedCell(Cell cell)
        {
	        cell.Display = Display.Hidden;
	        _flaggedCount--;

	        if (cell.State == State.Mine)
	        {
		        GameBoard.RemainingMineCount++;
	        }

	        return GameBoard;
        }

        private static GameBoard FlagOnHiddenCell(Cell cell)
        {
	        cell.Display = Display.Flag;
	        _flaggedCount++;

	        if (cell.State == State.Mine)
	        {
		        GameBoard.RemainingMineCount--;
	        }

	        if (GameBoard.RemainingMineCount == 0 && _flaggedCount == _initialMinesCount)
	        {
		        GameStatus = GameStatus.Win;
	        }

	        return GameBoard;
        }

		private static void Validate(int row, int column)
        {
	        if (GameBoard == null)
	        {
		        throw new ApplicationException("GameBoard is not generated, call GenerateGameBoard method first.");
	        }

	        if (row < 0 || row > GameBoard.Width - 1 || column < 0 || column > GameBoard.Height - 1)
	        {
		        throw new ArgumentOutOfRangeException($"row {row} or column {column} is out of the bounds" +
					$" of the generated board with height {GameBoard.Height} and width {GameBoard.Width}.");
	        }

	        if (GameStatus == GameStatus.Win)
	        {
		        throw new ApplicationException("Game is already won.");
	        }

	        if (GameStatus == GameStatus.GameOver)
	        {
		        throw new ApplicationException("Game is already lost.");
	        }
        }
    }
}