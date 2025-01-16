using System;
using System.Collections.Generic;
using System.Linq;


namespace OmegaSudoku.GameLogic
{
    class SudokuController
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
            // this func will return a list of tuples of row, col positions inside a row
            List<(int, int)> rowCells = new List<(int, int)>();
            for (int i = 0; i < GameBoard.GetLength(1); i++)
            {
                rowCells.Add((rowLvl, i));
            }
            return rowCells;
        }

        public List<(int, int)> GetColumnCells(int columnNum)
        {
            // this func will return a list of tuples of row, col positions inside a column
            List<(int, int)> columnCells = new List<(int, int)>();
            for (int i = 0; i < GameBoard.GetLength(0); i++)
            {
                columnCells.Add((i , columnNum));
            }
            return columnCells; 
        }

        public List<(int, int)> GetCubeCells(int rowPos, int colPos)
        {
            // this func will return a list of tuples of row, col positons inside a cube
            // cube size is root of the board width / length
            // the top left cube is 0,0 the bottom right cube is 2,2
            int cubeSize = (int)Math.Sqrt(GameBoard.GetLength(0));
            int cubeCol = (colPos / cubeSize) * cubeSize;
            int cubeRow = (rowPos / cubeSize) * cubeSize;

            List<(int, int)> cubeCells = new List<(int, int)>();
            for (int rowAdd = 0; rowAdd < cubeSize; rowAdd++)
            {
                for (int colAdd = 0; colAdd < cubeSize; colAdd++)
                {
                    cubeCells.Add((cubeRow + rowAdd, cubeCol + colAdd));
                }
            }
            return cubeCells;

        }

        public int GetCellValue(int rowPos, int colPos)
        {
            // get the game cell value
            return GameBoard[rowPos, colPos].CellValue;
        }

        public bool IsValidMove(int rowPos, int colPos)
        {
            // this func will return true / false if the move is valid
            int cellValue = GetCellValue(rowPos, colPos);   
            return IsValidCol(colPos, cellValue) && IsValidRow(rowPos, cellValue) && IsValidCube(rowPos, colPos, cellValue);
        }

        public bool IsValidRow(int rowLvl, int cellValue)
        {
            // func that returns if a row is valid
            List<(int, int)> rowCells = GetRowCells(rowLvl);
            return CheckForDoubles(rowCells, cellValue);

        }

        public bool IsValidCol(int colNum, int cellValue)
        {
            // func that returns if a col is valid
            List<(int, int)> colCells = GetColumnCells(colNum);
            return CheckForDoubles(colCells, cellValue);
        }

        public bool IsValidCube(int rowPos, int colPos, int cellValue)
        {
            // func that returns if a cube is valid
            List<(int, int)> cubeCells = GetCubeCells(rowPos, colPos);
            return CheckForDoubles(cubeCells, cellValue);
        }

        public bool CheckForDoubles(List<(int, int)> cells, int cellValue)
        {
            // this func is used to check for illegal double cell values in a cell list
            int counter = 0;
            foreach ((int, int) cell in cells)
            {
                int cellRow = cell.Item1;
                int cellCol = cell.Item2;
                if (GetCellValue(cellRow, cellCol) == cellValue)
                {
                    counter++;  
                }
            }
            return counter == 1;
        } 
    }
}
