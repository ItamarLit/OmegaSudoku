using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.Exceptions;

namespace OmegaSudoku.GameLogic
{
    public class SudokuLogicHandler
    {
        /// <summary>
        /// This class will control all the game logic, including checks of the board and setting up the board
        /// </summary>
        
        // hold the board as private
        private readonly Icell[,] _gameBoard;
        // mrvArray instance
        private readonly Mrvdict _mrvDict;
        
        public SudokuLogicHandler(Icell[,] board, Mrvdict mrvInstance)
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
            foreach (Icell cell in GetAllBoardCells())
            {
                // check if the cell in the cube has the same value of the checked cell
                if (cell.GetCellValue() != 0 && !IsValidMove(cell.GetCellRow(), cell.GetCellCol()))
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
            IEnumerable<Icell> boardCells = GetAllBoardCells();
            foreach (var cell in boardCells)
            {
                // check if the cell in the cube has the same value of the checked cell
                if (!cell.IsCellEmpty())
                {
                    HashSet<Icell> affectedUnitCells = GetFilteredUnitCells(cell.GetCellRow(), cell.GetCellCol(), cell.GetCellValue());
                    DecreasePossibilites(affectedUnitCells, GetCellValue(cell.GetCellRow(), cell.GetCellCol()));
                }
            }
            // after setting the board up we can mrv array
            foreach (var cell in boardCells)
            {
                if (cell.GetCellValue() == 0)
                {
                    // insert the cell into the array
                    _mrvDict.InsertCell(cell);
                }
            }
        }


        private IEnumerable<Icell> GetAllBoardCells()
        {
            // this func will return a list of tuples of row, col positions of all cells on the board
            List<Icell> boardCells = new List<Icell>();
            for (int row = 0; row < _gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < _gameBoard.GetLength(1); col++) 
                {
                    boardCells.Add(_gameBoard[row, col]);
                }
            }
            return boardCells;
        }

        /// <summary>
        /// This func returns all of the cells in a give row lvl
        /// </summary>
        /// <param name="rowLvl"></param>
        /// <returns>A list of tuples of row, col positions of a row</returns>
        public IEnumerable<Icell> GetRowCells(int rowLvl)
        {
            List<Icell> rowCells = new List<Icell>();
            for (int i = 0; i < _gameBoard.GetLength(1); i++)
            {
                rowCells.Add(_gameBoard[rowLvl, i]);
            }
            return rowCells;
        }

        public IEnumerable<Icell> GetColumnCells(int columnNum)
        {
            List<Icell> columnCells = new List<Icell>();
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                columnCells.Add(_gameBoard[i , columnNum]);
            }
            return columnCells; 
        }

        /// <summary>
        /// This func will return a list of tuples of row, col positons inside a cube
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <returns></returns>
        public IEnumerable<Icell> GetCubeCells(int rowPos, int colPos)
        {
            // cube size is root of the board width / length
            // the top left cube is 0,0 the bottom right cube is 2,2
            int cubeSize = (int)Math.Sqrt(_gameBoard.GetLength(0));
            int cubeCol = (colPos / cubeSize) * cubeSize;
            int cubeRow = (rowPos / cubeSize) * cubeSize;

            List<Icell> cubeCells = new List<Icell>();
            for (int rowAdd = 0; rowAdd < cubeSize; rowAdd++)
            {
                for (int colAdd = 0; colAdd < cubeSize; colAdd++)
                {
                    cubeCells.Add(_gameBoard[cubeRow + rowAdd, cubeCol + colAdd]);
                }
            }
            return cubeCells;

        }

        private int GetCellValue(int rowPos, int colPos)
        {
            // get the game cell value
            return _gameBoard[rowPos, colPos].GetCellValue();
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
            return CheckForDoubles(GetRowCells(rowLvl), cellValue);

        }

        private bool IsValidCol(int colNum, int cellValue)
        {
            // func that returns if a col is valid
            return CheckForDoubles(GetColumnCells(colNum), cellValue);
        }

        private bool IsValidCube(int rowPos, int colPos, int cellValue)
        {
            // func that returns if a cube is valid
            return CheckForDoubles(GetCubeCells(rowPos, colPos), cellValue);
        }

        private bool CheckForDoubles(IEnumerable<Icell> cells, int cellValue)
        {
            // this func is used to check for illegal double cell values in a cell list
            bool foundAlready = false;
            foreach (Icell cell in cells)
            {
                if (cell.GetCellValue() == cellValue)
                {
                    if (foundAlready) return false;
                    foundAlready = true;
                }
            }
            return true;
        }

        /// <summary>
        /// This func returns a hashset of unit cells including the cell at row, col
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <returns>List of tuples of unit cells</returns>
        public HashSet<Icell> GetUnitCellsPos(int rowPos, int colPos)
        {
            // combine all the unit cells into one hashset
            HashSet<Icell> unitCells = GetRowCells(rowPos).ToHashSet();
            unitCells.UnionWith(GetColumnCells(colPos));
            unitCells.UnionWith(GetCubeCells(rowPos, colPos));
            return unitCells;
        }

        public void DecreasePossibilites(IEnumerable<Icell> affectedUnitCells, int valueToRemove)
        {
            // This func is used to reduce the board possibilites based on the current change
            foreach(Icell cell in affectedUnitCells)
            {
                // attempt to remove the value
                cell.DecreasePossibility(valueToRemove);
            }
        }

        /// <summary>
        /// This func returns a list of BoardCells neighbours that doesnt include row, col BoardCell and filled cells
        /// </summary>
        /// <param name="rowPos"></param>
        /// <param name="colPos"></param>
        /// <param name="filterValue"></param>
        /// <returns>List of boardCells</returns>
        public HashSet<Icell> GetFilteredUnitCells(int rowPos, int colPos, int filterValue) 
        {
            // the cell at row, col is not in the hash set
            HashSet<Icell> unitCells = GetUnitCellsPos(rowPos, colPos);
            foreach (Icell unitCell in unitCells) 
            {
                int cellRow = unitCell.GetCellRow();
                int cellCol = unitCell.GetCellCol();
                if (!(unitCell.HasValue(filterValue) && !(cellRow == rowPos && cellCol == colPos) && unitCell.IsCellEmpty()))
                {
                    // remove the invalid cell
                    unitCells.Remove(unitCell);
                }
            }
            return unitCells;
        }

        /// <summary>
        /// This func returns true if the update of the cell is invalid (has no value and 0 remaining possibilites)
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <returns>true if the update is invalid else false</returns>
        public bool IsInvalidUpdate(IEnumerable<Icell> affectedCells)
        {
            // This func is used to check the board after every update to look for illegal cells ( no possibilites and no value)
            foreach (Icell cell in affectedCells)
            {
                if (cell.GetPossibilites().Count() == 0 && cell.IsCellEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        public int CountEmptyNeighbours(IEnumerable<Icell> unitCells)
        {
            int count = 0;
            foreach(var cell in unitCells)
            {
                if (cell.IsCellEmpty())
                {
                    count++;
                }
            }
            return count;
        }

    }
}
