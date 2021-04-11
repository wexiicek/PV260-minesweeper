namespace PV260_Minesweeper
{
    public static class Helper
    {
        public static int[,] Directions = { { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 } };

        public static bool UncorrectSizeOfTheBoard(int width, int height)
        {
	        return width < 3 || width > 50 || height < 3 || height > 50;
        }

        public static bool CellIsOutOfTheBoardRange(int xDir, int yDir, int row, int column, int width, int height)
        {
	        return row + yDir < 0 || column + xDir < 0 || row + yDir > width - 1 || column + xDir > height - 1;
        }
    }
}