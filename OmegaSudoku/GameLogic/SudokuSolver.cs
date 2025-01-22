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

        private readonly Mrvdict _mrvDict;

        private readonly SudokuLogicHandler _logicHandler;

        private readonly Stack<StateChange> _stateChangesStack;

        public SudokuSolver(BoardCell[,] gameBoard, Mrvdict mrvInstance)
        {
            _board = gameBoard;
            _mrvDict = mrvInstance;
            // create the game logic handler
            _logicHandler = new SudokuLogicHandler(_board, _mrvDict);
            // set up the board
            _logicHandler.CheckInitalBoard();
            _logicHandler.SetInitailBoardPossibilites();
            _stateChangesStack = new Stack<StateChange>();
        }

        /// <summary>
        /// This is the main solve func used to attempt to solve the given board
        /// </summary>
        /// <returns>
        /// The func returns true if it solved the board and false if not
        /// </returns>
        /// 
        public bool Solve()
        {
            (int row, int col) = _mrvDict.GetLowestPossibilityCell();
            if (_mrvDict.IsEmptyMap((row, col)))
            {
                // if there are no more cells to fill the sudoku is solved
                return true;
            }
            HashSet<int> possibilites = _board[row, col].GetPossibilites();
            if (possibilites.Count > 0)
            {
                foreach (int potentialValue in possibilites)
                {
                    StateChange currentState = new StateChange();
                    if (TrySolveCell(potentialValue, row, col, currentState))
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        public bool TrySolveCell(int potentialValue, int row, int col, StateChange currentState)
        {
            currentState.CellValueChanges.Add((row, col, 0));
            _board[row, col].CellValue = potentialValue;
            // save the affected cell positions incase the attempt is wrong
            HashSet<BoardCell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
            SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
            DecreaseGamePossibilites(affectedCells, row, col, potentialValue);
            // save the current state
            _stateChangesStack.Push(currentState);

            // call hidden singles check here
            if (_logicHandler.IsInvalidUpdate(affectedCells))
            {
                // reset the array and add back possibilites
                ResetState(affectedCells, row, col, potentialValue);
                return false;
            }
            // Insert the affected cells back into the mrv array
            _mrvDict.UpdateMRVCells(affectedCells, true);
            if (Solve())
            {
                return true;
            }
            Backtrack(affectedCells, row, col, potentialValue);
            return false;
        }
        
        public void SetSinglePossibilityCells()
        {

        }

        //public bool Solve()
        //{
        //    // get the lowest possibility cell
        //    (int row, int col) = _mrvDict.GetLowestPossibilityCell();
        //    if (_mrvDict.IsEmptyMap((row, col)))
        //    {
        //        // if there are no more cells to fill the sudoku is solved
        //        return true;
        //    }
        //    HashSet<int> possibilites = _board[row, col].GetPossibilites();
        //    if (possibilites.Count > 0)
        //    {
        //        // go over all the potential values
        //        foreach (int potentialValue in possibilites)
        //        {
        //            StateChange currentState = new StateChange();
        //            currentState.CellValueChanges.Add((row, col, 0));
        //            _board[row, col].CellValue = potentialValue;
        //            // save the affected cell positions incase the attempt is wrong
        //            HashSet<BoardCell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
        //            SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
        //            // remove the possibilites
        //            _mrvDict.UpdateMRVCells(affectedCells, false);
        //            // remove the current cell
        //            _mrvDict.RemoveCell(_board[row, col]);
        //            _logicHandler.DecreasePossibilites(affectedCells, potentialValue);
                    
        //            _stateChangesStack.Push(currentState);

        //            // call hidden singles check here
        //            if (_logicHandler.IsInvalidUpdate(affectedCells))
        //            {
        //                // reset the array and add back possibilites
        //                ResetState(affectedCells, row, col, potentialValue);
        //            }
        //            else
        //            {
        //                _mrvDict.UpdateMRVCells(affectedCells, true);
        //                if (Solve())
        //                {
        //                    return true;
        //                }
        //                // remove old MRV cells
        //                _mrvDict.UpdateMRVCells(affectedCells, false);
        //                // reset the board
        //                ResetState(affectedCells, row, col, potentialValue);
                        
        //            }
        //        }
        //    }
        //    return false;
        //}

        public void DecreaseGamePossibilites(HashSet<BoardCell> affectedCells, int row, int col, int potentialValue)
        {
            // remove the possibilites
            _mrvDict.UpdateMRVCells(affectedCells, false);
            // remove the current cell
            _mrvDict.RemoveCell(_board[row, col]);
            _logicHandler.DecreasePossibilites(affectedCells, potentialValue);
        }

        private void Backtrack(HashSet<BoardCell> affectedCells, int row, int col, int potentialValue)
        {
            _mrvDict.UpdateMRVCells(affectedCells, false);
            ResetState(affectedCells, row, col, potentialValue);
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
            ResetCellsUsingStack();
            _mrvDict.UpdateMRVCells(affectedCells, true);
            // insert the current cell
            _mrvDict.InsertCell(_board[row, col]);

        }

        private void ResetCellsUsingStack()
        {
            StateChange oldState = _stateChangesStack.Pop();
            foreach ((int, int, int) cellValueChange in oldState.CellValueChanges)
            {
                int row = cellValueChange.Item1;
                int col = cellValueChange.Item2;
                int value = cellValueChange.Item3;
                _board[row, col].CellValue = value;
            }
            foreach ((int, int, int) removedPossibility in oldState.CellPossibilityChanges)
            {
                int row = removedPossibility.Item1;
                int col = removedPossibility.Item2;
                int removedValue = removedPossibility.Item3;
                _board[row, col].IncreasePossibility(removedValue);
            }
        }

        private void SetAffectedCellsInStack(StateChange currentState, HashSet<BoardCell> affectedCells, int removedPossibility)
        {
            foreach (BoardCell cell in affectedCells)
            {
                currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, removedPossibility));
            }
        }

        
    }
}
