using System;
using System.Collections.Generic;
using System.Linq;


namespace OmegaSudoku.GameLogic
{
    class SudokuSolver
    {
        /// <summary>
        /// This class holds the main solve func that is used to attempt to solve the board and other helper funcs
        /// </summary>

        private readonly BoardCell[,] _board;

        private readonly MrvArray _mrvArray;

        private readonly SudokuLogicHandler _logicHandler;

        public SudokuSolver(BoardCell[,] gameBoard, MrvArray mrvInstance)
        {
            _board = gameBoard;
            _mrvArray = mrvInstance;
            // create the game logic handler
            _logicHandler = new SudokuLogicHandler(_board, _mrvArray);
            // set up the board
            _logicHandler.CheckInitalBoard();
            _logicHandler.SetInitailBoardPossibilites();
        }

        public bool Solve()
        {
            // This is the main solve func used to attempt to solve the given board
            // get the lowest possibility cell
            (int row, int col) = _mrvArray.GetLowestPossibilityCell();
            if (IsEmptyArray((row, col)))
            {
                // if there are no more cells to fill the sudoku is solved
                return true;
            }
            // get the first value from the possibilites
            List<int> possibilites = _board[row, col].GetPossibilites();
            if (possibilites.Count > 0)
            {
                // go over all the potential values
                foreach (int potentialValue in possibilites)
                {
                    // set the cell value but leave the possibilites in case i need to return
                    _board[row, col].CellValue = potentialValue;
                    // save the affected cell positions incase the attempt is wrong
                    List<(int, int)> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
                    // remove the possibilites
                    _mrvArray.RemoveAffectedMRVCells(GetAffectedCells(affectedCells));
                    // remove the current cell
                    _mrvArray.RemoveCell(_board[row, col]);
                    _logicHandler.DecreasePossibilites(affectedCells, potentialValue);
                    // If the func returns false this means there has been a mistake or the board is unsolvable
                    // because the func returns false only when a cell that has no value also has no possible value
                    if (_logicHandler.IsInvalidUpdate(affectedCells))
                    {
                        // need to reset the array and add back possibilites here
                        ResetState(affectedCells, row, col, potentialValue);
                    }
                    else
                    {
                        // insert the cells into place if the board is invalid
                        _mrvArray.InsertAffectedMRVCells(GetAffectedCells(affectedCells));
                        if (Solve())
                        {
                            return true;
                        }
                        // remove old MRV cells
                        _mrvArray.RemoveAffectedMRVCells(GetAffectedCells(affectedCells));
                        // got false from backtracking need to reset the board
                        ResetState(affectedCells, row, col, potentialValue);
                    }
                  
                }
            }
            return false;
        }

        public bool IsEmptyArray((int, int) rowColTuple)
        {
            if (rowColTuple.Item1 == -1 && rowColTuple.Item2 == -1)
            {
                return true;
            }
            return false;
        }

        private List<BoardCell> GetAffectedCells(List<(int, int)> affectedCells)
        {

            List<BoardCell> affectedBoardCells = new List<BoardCell>();
            foreach ((int row1, int col1) in affectedCells)
            {
                // remove the cell from the mrv array
                affectedBoardCells.Add(_board[row1, col1]);

            }
            return affectedBoardCells;
        }

        private void ResetState(List<(int, int)> affectedCells, int row, int col, int potentialValue)
        {
            _board[row, col].CellValue = 0;
            _logicHandler.IncreasePossibilites(affectedCells, potentialValue);
            _mrvArray.InsertAffectedMRVCells(GetAffectedCells(affectedCells));
            // insert the current cell
            _mrvArray.InsertCell(_board[row, col]);

        }

    }
}
