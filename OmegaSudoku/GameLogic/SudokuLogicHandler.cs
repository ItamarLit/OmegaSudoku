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
        private readonly MrvArray _mrvArray;

        public SudokuLogicHandler(BoardCell[,] board, MrvArray mrvInstance)
        {
            _gameBoard = board;
            // create the mrv array
            _mrvArray = mrvInstance;
        }

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

        public void SetInitailBoardPossibilites()
        {
            // set the board possibilites
            foreach ((int cellX, int cellY) in GetAllBoardCells())
            {
                // check if the cell in the cube has the same value of the checked cell
                if (GetCellValue(cellX, cellY) != 0)
                {
                    List<(int, int)> affectedUnitCells = GetFilteredUnitCells(cellX, cellY, GetCellValue(cellX, cellY));
                    DecreasePossibilites(affectedUnitCells, GetCellValue(cellX, cellY));
                }
            }
            // after setting the board up we can mrv array
            foreach ((int cellX, int cellY) in GetAllBoardCells())
            {
                if (GetCellValue(cellX, cellY) == 0)
                {
                    // insert the cell into the array
                    _mrvArray.InsertCell(_gameBoard[cellX, cellY]);
                }
            }
        }


        private List<(int, int)> GetAllBoardCells()
        {
            // this func will return a list of tuples of row, col positions of all cells on the board
            List<(int, int)> boardCells = new List<(int, int)>();
            for (int row = 0; row < _gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < _gameBoard.GetLength(1); col++) 
                {
                    boardCells.Add((row, col));
                }
            }
            return boardCells;
        }

        private List<(int, int)> GetRowCells(int rowLvl)
        {
            // this func will return a list of tuples of row, col positions inside a row
            List<(int, int)> rowCells = new List<(int, int)>();
            for (int i = 0; i < _gameBoard.GetLength(1); i++)
            {
                rowCells.Add((rowLvl, i));
            }
            return rowCells;
        }

        private List<(int, int)> GetColumnCells(int columnNum)
        {
            // this func will return a list of tuples of row, col positions inside a column
            List<(int, int)> columnCells = new List<(int, int)>();
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                columnCells.Add((i , columnNum));
            }
            return columnCells; 
        }

        private List<(int, int)> GetCubeCells(int rowPos, int colPos)
        {
            // this func will return a list of tuples of row, col positons inside a cube
            // cube size is root of the board width / length
            // the top left cube is 0,0 the bottom right cube is 2,2
            int cubeSize = (int)Math.Sqrt(_gameBoard.GetLength(0));
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

        private int GetCellValue(int rowPos, int colPos)
        {
            // get the game cell value
            return _gameBoard[rowPos, colPos].CellValue;
        }

        public bool IsValidMove(int rowPos, int colPos)
        {
            // this func will return true / false if the move is valid
            int cellValue = GetCellValue(rowPos, colPos);   
            return IsValidCol(colPos, cellValue) && IsValidRow(rowPos, cellValue) && IsValidCube(rowPos, colPos, cellValue);
        }

        private bool IsValidRow(int rowLvl, int cellValue)
        {
            // func that returns if a row is valid
            List<(int, int)> rowCells = GetRowCells(rowLvl);
            return CheckForDoubles(rowCells, cellValue);

        }

        private bool IsValidCol(int colNum, int cellValue)
        {
            // func that returns if a col is valid
            List<(int, int)> colCells = GetColumnCells(colNum);
            return CheckForDoubles(colCells, cellValue);
        }

        private bool IsValidCube(int rowPos, int colPos, int cellValue)
        {
            // func that returns if a cube is valid
            List<(int, int)> cubeCells = GetCubeCells(rowPos, colPos);
            return CheckForDoubles(cubeCells, cellValue);
        }

        private bool CheckForDoubles(List<(int, int)> cells, int cellValue)
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

        public List<(int, int)> GetUnitCellsPos(int rowPos, int colPos)
        {
            // combine all the unit cells using union to get rid of duplicates
            List<(int, int)> unitCells = new List<(int, int)>();
            unitCells = GetRowCells(rowPos).Union(GetColumnCells(colPos)).Union(GetCubeCells(rowPos, colPos)).ToList();
            return unitCells;
        }

        public void DecreasePossibilites(List<(int, int)> affectedUnitCells, int valueToRemove)
        {
            // This func is used to reduce the board possibilites based on the current change
            for (int i = 0; i < affectedUnitCells.Count; i++)
            {
                int cellRow = affectedUnitCells[i].Item1;
                int cellCol = affectedUnitCells[i].Item2;
                // attempt to remove the value
                _gameBoard[cellRow, cellCol].DecreasePossibility(valueToRemove);
            }
        }

        public void IncreasePossibilites(List<(int, int)> affectedUnitCells, int valueToReturn)
        {
            // This func is used to increase the board possibilites based on the current change
            for (int i = 0; i < affectedUnitCells.Count; i++)
            {
                int cellRow = affectedUnitCells[i].Item1;
                int cellCol = affectedUnitCells[i].Item2;
                // attempt to remove the value
                if (cellRow == 3 && cellCol == 7)
                {
                    Console.WriteLine($"Increasing: {valueToReturn}");
                }
                _gameBoard[cellRow, cellCol].IncreasePossibility(valueToReturn);
            }
        }

        public List<(int, int)> GetFilteredUnitCells(int rowPos, int colPos, int filterValue) 
        {
            // func that gets a filtered list of unit cells with no dups and no cells without the filter num as a possibility
            // the cell at row, col is not in the list
            List<(int, int)> unitCells = GetUnitCellsPos(rowPos, colPos);
            List<(int, int)> filteredCells = new List<(int, int)>();
            foreach ((int, int) unitCellTuple in unitCells) 
            {
                int cellRow = unitCellTuple.Item1;
                int cellCol = unitCellTuple.Item2;
                if (_gameBoard[cellRow, cellCol].HasValue(filterValue) && !(cellRow == rowPos && cellCol == colPos) && _gameBoard[cellRow, cellCol].CellIsEmpty()) 
                {
                    filteredCells.Add(unitCellTuple);
                }
            }
            return filteredCells;
        }

        public bool IsInvalidUpdate(List<(int, int)> affectedCells)
        {
            // This func is used to check the board after every update to look for illegal cells ( no possibilites and no value)
            foreach ((int row, int col) in affectedCells)
            {
                if (_gameBoard[row, col].GetPossibilites().Count == 0 && _gameBoard[row, col].CellValue == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
