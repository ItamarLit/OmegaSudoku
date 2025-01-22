using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.Exceptions;

namespace OmegaSudoku.GameLogic
{
    class SudokuLogicHandler
    {
        /// <summary>
        /// This class will control all the game logic, including checks of the board and setting up the board
        /// </summary>
        
        // hold the board as private
        private readonly BoardCell[,] _gameBoard;
        // mrvArray instance
        private readonly Mrvdict _mrvDict;

        public SudokuLogicHandler(BoardCell[,] board, Mrvdict mrvInstance)
        {
            _gameBoard = board;
            _mrvDict = mrvInstance;
        }

        /// <summary>
        /// This func checks the initial user inputed board and throws an exception if it is invalid
        /// </summary>
        /// <exception cref="InvalidInitialBoardException"></exception>
        public void CheckInitalBoard()
        {
            // go over all the filled cells and check that they are valid
            foreach ((int cellRow, int cellCol) in GetAllBoardCells())
            {
                // check if the cell in the cube has the same value of the checked cell
                if (GetCellValue(cellRow, cellCol) != 0 && !IsValidMove(cellRow, cellCol))
                {
                    // Throw invalid initial board exception
                    throw new InvalidInitialBoardException();
                }
            }
        }

        /// <summary>
        /// This func sets up the board and mrv with the initial possibilites after purging invalid ones
        /// </summary>
        public void SetInitailBoardPossibilites()
        {
            // set the board possibilites
            HashSet<(int, int)> boardCells = GetAllBoardCells();
            foreach ((int cellX, int cellY) in boardCells)
            {
                // check if the cell in the cube has the same value of the checked cell
                if (GetCellValue(cellX, cellY) != 0)
                {
                    HashSet<BoardCell> affectedUnitCells = GetFilteredUnitCells(cellX, cellY, GetCellValue(cellX, cellY));
                    DecreasePossibilites(affectedUnitCells, GetCellValue(cellX, cellY));
                }
            }
            // after setting the board up we can mrv array
            foreach ((int cellX, int cellY) in boardCells)
            {
                if (GetCellValue(cellX, cellY) == 0)
                {
                    // insert the cell into the array
                    _mrvDict.InsertCell(_gameBoard[cellX, cellY]);
                }
            }
        }

        private HashSet<(int, int)> GetAllBoardCells()
        {
            // this func will return a list of tuples of row, col positions of all cells on the board
            HashSet<(int, int)> boardCells = new HashSet<(int, int)>();
            for (int row = 0; row < _gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < _gameBoard.GetLength(1); col++) 
                {
                    boardCells.Add((row, col));
                }
            }
            return boardCells;
        }

        /// <summary>
        /// This func returns all of the cells in a give row lvl
        /// </summary>
        /// <param name="rowLvl"></param>
        /// <returns>A list of tuples of row, col positions of a row</returns>
        private HashSet<(int, int)> GetRowCells(int rowLvl)
        {
            // this func will return a list of tuples of row, col positions inside a row
            HashSet<(int, int)> rowCells = new HashSet<(int, int)>();
            for (int i = 0; i < _gameBoard.GetLength(1); i++)
            {
                rowCells.Add((rowLvl, i));
            }
            return rowCells;
        }

        private HashSet<(int, int)> GetColumnCells(int columnNum)
        {
            // this func will return a list of tuples of row, col positions inside a column
            HashSet<(int, int)> columnCells = new HashSet<(int, int)>();
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                columnCells.Add((i , columnNum));
            }
            return columnCells; 
        }

        private HashSet<(int, int)> GetCubeCells(int rowPos, int colPos)
        {
            // this func will return a list of tuples of row, col positons inside a cube
            // cube size is root of the board width / length
            // the top left cube is 0,0 the bottom right cube is 2,2
            int cubeSize = (int)Math.Sqrt(_gameBoard.GetLength(0));
            int cubeCol = (colPos / cubeSize) * cubeSize;
            int cubeRow = (rowPos / cubeSize) * cubeSize;

            HashSet<(int, int)> cubeCells = new HashSet<(int, int)>();
            for (int rowAdd = 0; rowAdd < cubeSize; rowAdd++)
            {
                for (int colAdd = 0; colAdd < cubeSize; colAdd++)
                {
                    cubeCells.Add((cubeRow + rowAdd, cubeCol + colAdd));
                }
            }
            return cubeCells;

        }

        private int GetCellValue(int rowPos, int colPos)
        {
            // get the game cell value
            return _gameBoard[rowPos, colPos].CellValue;
        }

        /// <summary>
        /// This func checks if a cell at pos (row, col) is valid to be there
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <returns>Returns true if the pos of cell is valid else returns false</returns>
        public bool IsValidMove(int rowPos, int colPos)
        {
            // this func will return true / false if the move is valid
            int cellValue = GetCellValue(rowPos, colPos);   
            return IsValidCol(colPos, cellValue) && IsValidRow(rowPos, cellValue) && IsValidCube(rowPos, colPos, cellValue);
        }

        private bool IsValidRow(int rowLvl, int cellValue)
        {
            // func that returns if a row is valid
            HashSet<(int, int)> rowCells = GetRowCells(rowLvl);
            return CheckForDoubles(rowCells, cellValue);

        }

        private bool IsValidCol(int colNum, int cellValue)
        {
            // func that returns if a col is valid
            HashSet<(int, int)> colCells = GetColumnCells(colNum);
            return CheckForDoubles(colCells, cellValue);
        }

        private bool IsValidCube(int rowPos, int colPos, int cellValue)
        {
            // func that returns if a cube is valid
            HashSet<(int, int)> cubeCells = GetCubeCells(rowPos, colPos);
            return CheckForDoubles(cubeCells, cellValue);
        }

        private bool CheckForDoubles(HashSet<(int, int)> cells, int cellValue)
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

        /// <summary>
        /// This func returns a hashset of unit cells including the cell at row, col
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <returns>List of tuples of unit cells</returns>
        public HashSet<(int, int)> GetUnitCellsPos(int rowPos, int colPos)
        {
            // combine all the unit cells into one hashset
            HashSet<(int, int)> unitCells = GetRowCells(rowPos);
            unitCells.UnionWith(GetColumnCells(colPos));
            unitCells.UnionWith(GetCubeCells(rowPos, colPos));
            return unitCells;
        }

        public void DecreasePossibilites(HashSet<BoardCell> affectedUnitCells, int valueToRemove)
        {
            // This func is used to reduce the board possibilites based on the current change
            foreach(BoardCell cell in affectedUnitCells)
            {
                int cellRow = cell.CellRow;
                int cellCol = cell.CellCol;
                // attempt to remove the value
                _gameBoard[cellRow, cellCol].DecreasePossibility(valueToRemove);
            }
        }

        public void IncreasePossibilites(HashSet<BoardCell> affectedUnitCells, int valueToReturn)
        {
            // This func is used to increase the board possibilites based on the current change
            foreach (BoardCell cell in affectedUnitCells)
            {
                int cellRow = cell.CellRow;
                int cellCol = cell.CellCol;
                // attempt to remove the value
                _gameBoard[cellRow, cellCol].IncreasePossibility(valueToReturn);
            }
        }

        /// <summary>
        /// This func returns a list of BoardCells neighbours that doesnt include row, col BoardCell
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <param name="filterValue"></param>
        /// <returns>List of boardCells</returns>
        public HashSet<BoardCell> GetFilteredUnitCells(int rowPos, int colPos, int filterValue) 
        {
            // func that gets a filtered list of unit cells with no dups and no cells without the filter num as a possibility
            // the cell at row, col is not in the list
            HashSet<(int, int)> unitCells = GetUnitCellsPos(rowPos, colPos);
            HashSet<BoardCell> filteredCells = new HashSet<BoardCell>();
            foreach ((int, int) unitCellTuple in unitCells) 
            {
                int cellRow = unitCellTuple.Item1;
                int cellCol = unitCellTuple.Item2;
                if (_gameBoard[cellRow, cellCol].HasValue(filterValue) && !(cellRow == rowPos && cellCol == colPos) && _gameBoard[cellRow, cellCol].CellIsEmpty()) 
                {
                    filteredCells.Add(_gameBoard[cellRow, cellCol]);
                }
            }
            return filteredCells;
        }

        /// <summary>
        /// This func returns true if the update of the cell is invalid (has no value and 0 remaining possibilites)
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <returns>true if the update is invalid else false</returns>
        public bool IsInvalidUpdate(HashSet<BoardCell> affectedCells)
        {
            // This func is used to check the board after every update to look for illegal cells ( no possibilites and no value)
            foreach (BoardCell cell in affectedCells)
            {
                if (_gameBoard[cell.CellRow, cell.CellCol].GetPossibilites().Count == 0 && _gameBoard[cell.CellRow, cell.CellCol].CellValue == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
