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

        /// <summary>
        /// This is the main solve func used to attempt to solve the given board
        /// </summary>
        /// <returns>
        /// The func returns true if it solved the board and false if not
        /// </returns>
        public bool Solve()
        {
            // get the lowest possibility cell
            (int row, int col) = _mrvArray.GetLowestPossibilityCell();
            if (_mrvArray.IsEmptyArray((row, col)))
            {
                // if there are no more cells to fill the sudoku is solved
                return true;
            }
            List<int> possibilites = _board[row, col].GetPossibilites();
            if (possibilites.Count > 0)
            {
                // go over all the potential values
                foreach (int potentialValue in possibilites)
                {
                    // set the cell value but leave the possibilites in case i need to return
                    _board[row, col].CellValue = potentialValue;
                    // save the affected cell positions incase the attempt is wrong
                    HashSet<BoardCell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
                    // remove the possibilites
                    _mrvArray.RemoveAffectedMRVCells(affectedCells);
                    // remove the current cell
                    _mrvArray.RemoveCell(_board[row, col]);
                    _logicHandler.DecreasePossibilites(affectedCells, potentialValue);
                    if (_logicHandler.IsInvalidUpdate(affectedCells))
                    {
                        // reset the array and add back possibilites
                        ResetState(affectedCells, row, col, potentialValue);
                    }
                    else
                    {
                        _mrvArray.InsertAffectedMRVCells(affectedCells);
                        if (Solve())
                        {
                            return true;
                        }
                        // remove old MRV cells
                        _mrvArray.RemoveAffectedMRVCells(affectedCells);
                        // reset the board
                        ResetState(affectedCells, row, col, potentialValue);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This func resets the game board after a failed backtracking attempt
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        private void ResetState(HashSet<BoardCell> affectedCells, int row, int col, int potentialValue)
        {
            _board[row, col].CellValue = 0;
            _logicHandler.IncreasePossibilites(affectedCells, potentialValue);
            _mrvArray.InsertAffectedMRVCells(affectedCells);
            // insert the current cell
            _mrvArray.InsertCell(_board[row, col]);

        }

    }
}
