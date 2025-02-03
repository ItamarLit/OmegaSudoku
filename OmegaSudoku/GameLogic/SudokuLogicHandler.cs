using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.Exceptions;
using OmegaSudoku.Interfaces;


namespace OmegaSudoku.GameLogic
{
    public class SudokuLogicHandler
    {
        /// <summary>
        /// This class will control all the game logic, including checks of the board and setting up the board
        /// </summary>
        
        // hold the board as private
        private readonly Icell[,] _gameBoard;
        // mrvDict instance
        private readonly Mrvdict _mrvDict;
        // caches for the cells in the different units
        private List<Icell>[] _rowsCache;
        private List<Icell>[] _columnsCache;
        private List<Icell>[] _cubesCache;
        private Dictionary<Icell, HashSet<Icell>> _neighboursCache;
        
        public SudokuLogicHandler(Icell[,] board, Mrvdict mrvInstance)
        {
            _gameBoard = board;
            _mrvDict = mrvInstance;
            int boardSize = board.GetLength(0);
            InitCaches(boardSize);
            _neighboursCache = new Dictionary<Icell, HashSet<Icell>>();
        }

        /// <summary>
        /// This func will init the caches to hold all the correct cells
        /// </summary>
        /// <param name="boardSize"></param>
        private void InitCaches(int boardSize)
        {
            _rowsCache = new List<Icell>[boardSize];
            _columnsCache = new List<Icell>[boardSize];
            _cubesCache = new List<Icell>[boardSize];
            for (int i = 0; i < boardSize; i++)
            {
                _rowsCache[i] = new List<Icell>();
                _columnsCache[i] = new List<Icell>();
                _cubesCache[i] = new List<Icell>();
            }
            FillCaches(boardSize);
        }

        private void FillCaches(int boardSize)
        {
            // this func is used to fill the caches with the correct cells
            int cubeSize = (int)Math.Sqrt(boardSize);
            for (int row = 0; row < boardSize; row++) 
            {
                for(int col = 0; col < boardSize; col++)
                {
                    Icell cell = _gameBoard[row, col];
                    _rowsCache[row].Add(cell);
                    _columnsCache[col].Add(cell);
                    int cubeIndex = SolverUtils.GetCubeIndex(row, col, boardSize);
                    _cubesCache[cubeIndex].Add(cell);
                }
            }
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
                if (cell.CellValue != 0 && !IsValidMove(cell.CellRow, cell.CellCol))
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
                // decrease the possibilites based on filled values
                if (!cell.IsCellEmpty())
                {
                    HashSet<Icell> affectedUnitCells = GetFilteredUnitCells(cell.CellRow, cell.CellCol, cell.CellValue);
                    DecreasePossibilites(affectedUnitCells, cell.CellValue);
                }
            }
            // after setting the board up we can set the mrv dict
            foreach (var cell in boardCells)
            {
                if (cell.CellValue == 0)
                {
                    // insert the cell into the dict
                    _mrvDict.InsertCell(cell);
                }
            }
        }


        private IEnumerable<Icell> GetAllBoardCells()
        {
            // this func will return a list of all cells on the board
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

        public IEnumerable<Icell> GetRowCells(int rowLvl)
        {
            return _rowsCache[rowLvl];
        }

        public IEnumerable<Icell> GetColumnCells(int columnNum)
        {
            return _columnsCache[columnNum];
        }

        public IEnumerable<Icell> GetCubeCells(int rowPos, int colPos)
        {
            int cubeIndex = SolverUtils.GetCubeIndex(rowPos, colPos, _gameBoard.GetLength(0));
            return _cubesCache[cubeIndex];
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
            int cellValue = _gameBoard[rowPos, colPos].CellValue;   
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
                if (cell.CellValue == cellValue)
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
        /// <returns>HashSet of unit cells</returns>
        public HashSet<Icell> GetUnitCellsPos(int rowPos, int colPos)
        {
            Icell cell = _gameBoard[rowPos, colPos];
            if(!_neighboursCache.TryGetValue(cell, out var set))
            {
                set = GetRowCells(rowPos).ToHashSet();
                set.UnionWith(GetColumnCells(colPos));
                set.UnionWith(GetCubeCells(rowPos, colPos));
                _neighboursCache[cell] = set;
            }
            return set;
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
            HashSet<Icell> filteredCells = new HashSet<Icell>();
            foreach (Icell unitCell in unitCells) 
            {
                int cellRow = unitCell.CellRow;
                int cellCol = unitCell.CellCol;
                if (unitCell.HasValue(filterValue) && !(cellRow == rowPos && cellCol == colPos) && unitCell.IsCellEmpty())
                {
                    // add the cell
                    filteredCells.Add(unitCell);
                }
            }
            return filteredCells;
        }

        /// <summary>
        /// This func returns true if the update of the cell is invalid (has no value and 0 remaining possibilites)
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <returns>true if the update is invalid else false</returns>
        public bool IsInvalidUpdate(IEnumerable<Icell> affectedCells)
        {
            foreach (Icell cell in affectedCells)
            {
                if (cell.NumberOfPossibilites() == 0 && cell.IsCellEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        public int CountEmptyNeighbours(IEnumerable<Icell> unitCells)
        {
            // this func counts all empty cells in unitCells
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
