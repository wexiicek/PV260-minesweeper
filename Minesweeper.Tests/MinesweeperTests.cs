using System;
using System.Linq;
using NUnit.Framework;
using PV260_Minesweeper;

namespace MinesweeperTests
{
	[TestFixture]
    public class MinesweeperTests
    {
        [TestCase(2, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 2)]
        [TestCase(50, 51)]
        [TestCase(51, 50)]
        [TestCase(51, 51)]
        public void GenerateGameBoard_InvalidSize_ThrowsException(int width, int height)
        {
	        Assert.Throws<ArgumentOutOfRangeException>(() => Minesweeper.GenerateGameBoard(width, height));
        }

        [TestCase(3, 3)]
        [TestCase(25, 25)]
        [TestCase(50, 50)]
        public void GenerateGameBoard_ValidSize_ReturnsBoard(int width, int height)
        {
	        var board = Minesweeper.GenerateGameBoard(width, height);

            Assert.IsInstanceOf<GameBoard>(board);
        }
        
        [TestCase(3, 3)]
        [TestCase(6, 9)]
        public void GenerateGameBoard_ValidSize_ReturnsValidBoard(int width, int height)
        {
            
            var board = Minesweeper.GenerateGameBoard(width, height);

            var boardWidth = board.Width;
            var boardHeight = board.Height;

            Assert.AreEqual(width, boardWidth);
            Assert.AreEqual(height, boardHeight);
            
            Assert.AreEqual(boardWidth * boardHeight, board.Board.Length);
        }
        
        [Test]
        public void GenerateGameBoard_ValidSize_GameStatusSetToInProgress()
        {
            Minesweeper.GenerateGameBoard(10, 10);

            Assert.AreEqual(GameStatus.InProgress, Minesweeper.GameStatus);
        }


        

        [Test]

        public void GenerateGameBoard_ValidSize_AllCellsAreHidden()
        {
            Minesweeper.GenerateGameBoard(10, 10);

            var board = Minesweeper.GameBoard.Board;
            
            var allCells = board.Cast<Cell>().Select(x => x.Display);
            
            Assert.AreEqual(1, allCells.Distinct().Count());
            Assert.AreEqual(Display.Hidden, board[0, 0].Display);
        }   
        
        [Test]
        public void Uncover_VisibleCell_DoNothing()
        {
            Minesweeper.GenerateGameBoard(10, 10);

            var board = Minesweeper.GameBoard.Board;

            board[2, 2].Display = Display.Visible;

            Minesweeper.Uncover(2, 2);
            
            Assert.AreEqual(Display.Visible, board[2, 2].Display);
        }

        [Test]
        public void Uncover_FlaggedCell_DoNothing()
        {
            Minesweeper.GenerateGameBoard(10, 10);

            var board = Minesweeper.GameBoard.Board;

            board[2, 2].Display = Display.Flag;

            Minesweeper.Uncover(2, 2);
            
            Assert.AreEqual(Display.Flag, board[2, 2].Display);
        }

        [Test]
        public void Uncover_Mine_GameStatusSetToGameOver()
        {
            Minesweeper.GenerateGameBoard(10, 10);
            
            var board = Minesweeper.GameBoard.Board;

            board[2, 2].State = State.Mine;
            
            Minesweeper.Uncover(2, 2);
            
            Assert.AreEqual(GameStatus.GameOver, Minesweeper.GameStatus);
        }

        [Test]
        public void Uncover_MinesAround_SetCellToVisible()
        {
            Minesweeper.GenerateGameBoard(10, 10);

            var board = Minesweeper.GameBoard.Board;

            board[2, 2].State = State.MinesAround;

            Minesweeper.Uncover(2, 2);
            
            Assert.AreEqual(Display.Visible, board[2, 2].Display);
        }
        
        [Test]
        public void Uncover_GameBoardIsNotGenerated_ThrowsApplicationException()
        {
            Minesweeper.Reset();
            Assert.Throws<ApplicationException>(() => Minesweeper.Uncover(0, 0));
        }
        
        [TestCase(5, 5, 10, 10)]
        public void Uncover_IndexOutsideOfGameBoard_ThrowsArgumentOutOfRangeException(int width, int height, int row, int col)
        {
            Minesweeper.GenerateGameBoard(width, height);
            
            Assert.Throws<ArgumentOutOfRangeException>(() => Minesweeper.Uncover(row, col));
        }
        
        [Test]
        public void Flag_FlagAllMines_GameStatusIsWin()
        {
            Minesweeper.GenerateGameBoard(3, 3);

            var board = Minesweeper.GameBoard.Board;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j].State == State.Mine)
                    {
                        Minesweeper.Flag(i, j);
                    }
                }
            }

            Assert.AreEqual(GameStatus.Win, Minesweeper.GameStatus);
        }

        [Test]
        public void Flag_FlaggedEmptyCell_CellIsHidden()
        {
            Minesweeper.GenerateGameBoard(3, 3);

            var board = Minesweeper.GameBoard.Board;

            board[0, 0].Display = Display.Flag;
            board[0, 0].State = State.Empty;

            Minesweeper.Flag(0, 0);
            
            Assert.AreEqual(Display.Hidden, board[0, 0].Display);
        }
        
        [Test]
        public void Flag_FlaggedMinedCell_RemainingMinesCountIncreasesAndCellIsHidden()
        {
            Minesweeper.GenerateGameBoard(3, 3);

            var board = Minesweeper.GameBoard.Board;

            board[0, 0].Display = Display.Flag;
            board[0, 0].State = State.Mine;

            var beforeMinesCount = Minesweeper.GameBoard.RemainingMineCount;

            Minesweeper.Flag(0, 0);
            
            Assert.AreEqual(Display.Hidden, board[0, 0].Display);
            Assert.AreEqual(beforeMinesCount+1, Minesweeper.GameBoard.RemainingMineCount);
        }
        
        [Test]
        public void Flag_VisibleCell_CellRemainsUnchanged()
        {
            Minesweeper.GenerateGameBoard(3, 3);

            var board = Minesweeper.GameBoard.Board;

            board[0, 0].Display = Display.Visible;

            Minesweeper.Flag(0, 0);
            
            Assert.AreEqual(Display.Visible, board[0, 0].Display);
        }
        
        [Test]
        public void Flag_HiddenMinedCell_RemainingMinesCountDecreasesAndCellIsFlagged()
        {
            Minesweeper.GenerateGameBoard(3, 3);

            var board = Minesweeper.GameBoard.Board;
            
            board[0, 0].State = State.Mine;

            var beforeMinesCount = Minesweeper.GameBoard.RemainingMineCount;

            Minesweeper.Flag(0, 0);
            
            Assert.AreEqual(Display.Flag, board[0, 0].Display);
            Assert.AreEqual(beforeMinesCount-1, Minesweeper.GameBoard.RemainingMineCount);
        }
    }
}