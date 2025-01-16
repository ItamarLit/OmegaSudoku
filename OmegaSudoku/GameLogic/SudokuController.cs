using System;
using System.Collections.Generic;
using System.Linq;


namespace OmegaSudoku.GameLogic
{
    internal class SudokuController
    {
        /// <summary>
        /// This class will control all the game logic, including checks of the board 
        /// and it will also include the function that will attempt to solve the board
        /// </summary>
        public BoardCell[,] GameBoard { get; }

        public SudokuController(BoardCell[,] board)
        {
            GameBoard = board;
        }

        public List<(int, int)> GetRowCells(int rowLvl)
        {
            return null;
        }

        public List<(int, int)> GetColumnCells(int columnNum)
        {
            return null;
        }

        public List<(int, int)> GetCubeCells(int x, int y)
        {
            return null;
        }

        public int GetCellValue(int xPos, int yPos)
        {
            // get the game cell value
            return GameBoard[xPos, yPos].CellValue;
        }

        public bool IsValidMove(int xPos, int yPos)
        {
            // this func will return true / false if the move is valid
            return false;
        }

    }
}
