namespace PV260_Minesweeper
{
    public class Cell
    {
        public int MinesAround { get; set; }
        public Visibility Visibility { get; set; }
        public CellState CellState { get; set; }
    }
}