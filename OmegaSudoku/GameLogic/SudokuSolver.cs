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
            // this is the main solve func
            // get the lowest possibility cell
            (int, int) lowestPossibilityCell = _mrvArray.GetLowestPossibilityCell();
            if (IsEmptyArray(lowestPossibilityCell)) 
            {
                // if there are no more cells to fill the sudoku is solved
                return true;  
            }
            int row = lowestPossibilityCell.Item1;
            int col = lowestPossibilityCell.Item2;
            // get the first value from the possibilites
            List<int> possibilites = _board[row, col].GetPossibilites();
            if(possibilites.Count > 0)
            {
                foreach (int potentialValue in possibilites)
                {
                    // set the cell value but leave the possibilites in case i need to return
                    _board[row, col].CellValue = potentialValue;
                    // save the affected cell positions incase the attempt is wrong
                    List<(int, int)> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
                    // remove the possibilites
                    foreach ((int row1, int col1) in affectedCells)
                    {
                        // remove the cell from the mrv array and put it in the correct place
                        _mrvArray.RemoveCell(_board[row1, col1]);
                      
                    }
                    _logicHandler.DecreasePossibilites(row, col, potentialValue);
                    // fix the mrvArray
                    foreach ((int row1, int col1) in affectedCells)
                    {
                        if (!_mrvArray.InsertCell(_board[row1, col1]))
                        {
                            return false;
                        }

                    }
                    if (Solve())
                    {
                        return true;
                    }
                    // got false from backtracking need to reset the board
                    _board[row, col].CellValue = 0;
                    foreach ((int row1, int col1) in affectedCells)
                    {
                        // remove the cell from the mrv array and put it in the correct place
                        _mrvArray.RemoveCell(_board[row1, col1]);

                    }
                    _logicHandler.IncreasePossibilites(affectedCells, potentialValue);
                    // fix the mrvArray
                    foreach ((int row1, int col1) in affectedCells)
                    {
                        if (!_mrvArray.InsertCell(_board[row1, col1]))
                        {
                            return false;
                        }

                    }
                }  
            }
            return false;
        }

        public bool IsEmptyArray((int, int) rowColTuple)
        {
            if(rowColTuple.Item1 == -1 && rowColTuple.Item2 == -1)
            {
                return true;
            }
            return false;
        }

        //private bool UpdateMrvArray(List<(int, int)> affectedCells)
        //{
        //    foreach ((int row, int col) in affectedCells)
        //    {
        //        // remove the cell from the mrv array and put it in the correct place
        //        _mrvArray.RemoveCell(_board[row, col]);
        //        if(!_mrvArray.InsertCell(_board[row, col])) 
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
