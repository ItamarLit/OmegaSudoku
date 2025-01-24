using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.GameLogic.Heurisitcs;

namespace OmegaSudoku.GameLogic
{
    public class SudokuSolver
    {
        /// <summary>
        /// This class holds the main solve func that is used to attempt to solve the board and other helper funcs
        /// </summary>

        private readonly BoardCell[,] _board;

        private readonly Mrvdict _mrvDict;

        private readonly SudokuLogicHandler _logicHandler;

        private readonly Stack<StateChange> _stateChangesStack;

        private (int , int)? _lastUpdatedCell;

        public static int depth = 0;

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
            depth++;
            int errorMet = ApplyHeuristics();
            if(errorMet == -1)
            {
                return false;
            }
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
                    _lastUpdatedCell = (row, col);
                    StateChange currentState = new StateChange();
                    if (TrySolveCell(potentialValue, row, col, currentState))
                    {
                        return true;
                    }
                }
            }
            if(errorMet == 1)
            {
                ResetState();
            }
            return false;
        }

        
        /// <summary>
        /// This func applys the different heuristics to the board
        /// 1. Naked singles
        /// 2. Hidden singles
        /// 3. Naked pairs
        /// </summary>
        /// <returns></returns>
        public int ApplyHeuristics()
        {
            StateChange currentState = new StateChange();
            bool metError = false;
            bool madeProgress = true;
            
            while (!metError && madeProgress)
            {
                int currentValueChanges = currentState.CellValueChanges.Count;
                int currentPossiblityChanges = currentState.CellPossibilityChanges.Count;
                // attempt to apply naked singles
                int addedHiddenSingles = SolveSinglePossibilityCells(currentState);
                if (addedHiddenSingles == -1)
                {
                    metError = true;
                    // need to push current state and pop it 
                }
                if (_lastUpdatedCell != null && !metError)
                {
                    int lastUpdatedRow = _lastUpdatedCell.Value.Item1;
                    int lastUpdatedCol = _lastUpdatedCell.Value.Item2;
                    HiddenSinglesUtil.ApplyHiddenSingles(currentState, lastUpdatedRow, lastUpdatedCol, _board, _logicHandler, _mrvDict);
                    // check if we were able to apply naked pairs
                    bool wasValid = NakedPairsUtil.ApplyNakedPairs(currentState, lastUpdatedRow, lastUpdatedCol, _board, _logicHandler, _mrvDict);
                    if (!wasValid)
                    {
                        metError = true;
                    }
                }
                if(currentState.CellValueChanges.Count - currentValueChanges <= 0 && currentState.CellPossibilityChanges.Count - currentPossiblityChanges <= 0)
                {
                    break;
                }
                madeProgress = currentState.CellValueChanges.Count  > 0 || currentState.CellPossibilityChanges.Count  > 0;

            }
            if(metError)
            {
                // push the current state object
                _stateChangesStack.Push(currentState);
                ResetState();
                return -1;
            }
            else if(!metError && madeProgress && currentState.CellValueChanges.Count != 0 || currentState.CellPossibilityChanges.Count != 0)
            {
                _stateChangesStack.Push(currentState);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// This func is used to place the naked singles on the board
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public int SolveSinglePossibilityCells(StateChange currentState)
        {
            int singleCellsAddedCount = 0;
            // run while there are naked singles on the board
            while (_mrvDict.HasSinglePossibiltyCell())
            {
                // get the row, col of the cell
                (int row, int col) = _mrvDict.GetLowestPossibilityCell();
                int potentialValue = _board[row, col].GetPossibilites().First();
                currentState.CellValueChanges.Add((row, col, _board[row, col].CellValue));
                _board[row, col].CellValue = potentialValue;
                // remove the possibility
                HashSet<BoardCell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
                SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
                DecreaseGamePossibilites(affectedCells, row, col, potentialValue);
                // check if the update was valid
                if (_logicHandler.IsInvalidUpdate(affectedCells))
                {
                    return -1;
                }
                _lastUpdatedCell = (row, col);
                _mrvDict.UpdateMRVCells(affectedCells, true);
                singleCellsAddedCount++;
            }
            return singleCellsAddedCount;
        }

        /// <summary>
        /// This func attepts to solve the board by placing a value in a cell and recursivly calling the solve
        /// </summary>
        /// <param name="potentialValue"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
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
                ResetState();
                return false;
            }
            // Insert the affected cells back into the mrv array
            _mrvDict.UpdateMRVCells(affectedCells, true);
            if (Solve())
            {
                return true;
            }
            // backtrack
            ResetState();
            return false;
        }


        /// <summary>
        /// This func is used to remove possiblites from the board after choosing a value to place in a cell
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        public void DecreaseGamePossibilites(IEnumerable<BoardCell> affectedCells, int row, int col, int potentialValue)
        {
            // remove the possibilites
            _mrvDict.UpdateMRVCells(affectedCells, false);
            // remove the current cell
            _mrvDict.RemoveCell(_board[row, col]);
            _logicHandler.DecreasePossibilites(affectedCells, potentialValue);
        }


        /// <summary>
        /// This func resets the game board after a failed backtracking attempt
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        private void ResetState()
        {
            HashSet<BoardCell> affectedCells = ResetCellsUsingStack();
            _mrvDict.UpdateMRVCells(affectedCells, true);
            _lastUpdatedCell = null;
        }

        /// <summary>
        /// This func will reset the state using the stack and the last state of the board
        /// </summary>
        /// <returns></returns>
        private HashSet<BoardCell> ResetCellsUsingStack()
        {
            StateChange oldState = _stateChangesStack.Pop();
            HashSet<BoardCell> changedCells = new HashSet<BoardCell>();
            foreach ((int row, int col, int value) in oldState.CellValueChanges)
            {
                BoardCell cell = _board[row, col];
                cell.CellValue = value;
                changedCells.Add(cell);
            }
            foreach ((int row, int col, int removedValue) in oldState.CellPossibilityChanges)
            {
                BoardCell cell = _board[row, col];
                if (!changedCells.Contains(cell))
                {
                    _mrvDict.RemoveCell(cell);
                    changedCells.Add(cell);
                }
                cell.IncreasePossibility(removedValue);
            }
            return changedCells;
        }

        /// <summary>
        /// This func sets the changed cells and there possiblities in the stack
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="affectedCells"></param>
        /// <param name="removedPossibility"></param>
        private void SetAffectedCellsInStack(StateChange currentState, IEnumerable<BoardCell> affectedCells, int removedPossibility)
        {
            foreach (BoardCell cell in affectedCells)
            {
                currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, removedPossibility));
            }
        }


    }
}
