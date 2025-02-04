using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.GameLogic.Heurisitcs;
using OmegaSudoku.Interfaces;

namespace OmegaSudoku.GameLogic
{
    public class SudokuSolver : ISolver
    {
        /// <summary>
        /// This class holds the main solve func that is used to attempt to solve the board and other helper funcs
        /// </summary>

        private readonly ICell[,] _board;

        private readonly Mrvdict _mrvDict;

        private readonly SudokuLogicHandler _logicHandler;

        private readonly Stack<StateChange> _stateChangesStack;
        // set the heuristics used to solve the board
        private readonly List<IHeuristic> _heuristics = new List<IHeuristic>
        {
            // hidden single and hidden pair
            new HiddenSets(1),
            new HiddenSets(2),
            // naked pair
            new NakedSets(2),
        };
 
        private ICell? _lastUpdatedCell;

        public SudokuSolver(ICell[,] gameBoard)
        {
            _board = gameBoard;
            // create mrv dict
            _mrvDict = new Mrvdict(_board.GetLength(0)); ;
            // create the game logic handler
            _logicHandler = new SudokuLogicHandler(_board, _mrvDict);
            // set up the board
            _logicHandler.CheckInitalBoard();
            _logicHandler.SetInitailBoardPossibilites();
            // set up stack to track changes
            _stateChangesStack = new Stack<StateChange>();
        }

        /// <summary>
        /// This is the main solve func used to attempt to solve the given board
        /// </summary>
        /// <returns>
        /// The func returns true if it solved the board and false if not
        /// </returns>
        public bool Solve()
        {
            // apply the heuristics to the board
            HeuristicResult heuristicRes = ApplyHeuristics();
            if(heuristicRes == HeuristicResult.InvalidState)
            {
                // invalid board 
                return false;
            }
            ICell cell = _mrvDict.GetLowestPossibilityCell(_logicHandler, _board);
            if (_mrvDict.IsEmptyMap(cell))
            {
                // if there are no more cells to fill the sudoku is solved
                return true;
            }
            // go over all the possibilities of a chosen cell 
            IEnumerable<int> possibilites = cell.GetPossibilites();
            if (possibilites.Count() > 0)
            {
                // attempt to solve the board
                foreach (int potentialValue in possibilites)
                {
                    _lastUpdatedCell = cell;
                    StateChange currentState = new StateChange();
                    if (TrySolveCell(potentialValue, cell, currentState))
                    {
                        return true;
                    }
                }
            }
            // if the guess fails reset the board to before heuristics
            if(heuristicRes == HeuristicResult.ProgressMade)
            {
                ResetState();
            }
            return false;
        }


        /// <summary>
        /// This func applys the different heuristics to the board, it checks if progress was made each time
        /// 1. Naked singles
        /// 2. Hidden singles
        /// 3. Hidden pairs
        /// 5. Naked pairs
        /// </summary>
        /// <returns></returns>
        public HeuristicResult ApplyHeuristics()
        {
            if (_board.GetLength(0) == 1)
            {
                // dont run heuristics for boards of one cell (no naked sets or hidden sets)
                return HeuristicResult.NoChange;
            }
            StateChange currentState = new StateChange();
            bool metError = false;
            bool madeProgress = true;
            // run loop until i get an error or until i stop changing 
            while (!metError && madeProgress)
            {
                int previousValueChanges = currentState.CellValueChanges.Count;
                int previousPossibilityChanges = currentState.CellPossibilityChanges.Count;
                // attempt to apply naked singles first
                bool addedHiddenSingles = NakedSingleUtil.SolveSinglePossibilityCells(currentState, _board, _mrvDict, ref _lastUpdatedCell, _logicHandler);
                if (!addedHiddenSingles)
                {
                    // invalid board after naked singles
                    metError = true;
                }
                if (_lastUpdatedCell != null && !metError)
                {
                    // apply each heuristic not including naked singles as it changes cell values
                    int lastUpdatedRow = _lastUpdatedCell.CellRow;
                    int lastUpdatedCol = _lastUpdatedCell.CellCol;
                    for (int index = 0; index < _heuristics.Count && !metError; index++) 
                    {
                        // apply all heuristics in the list
                        if (!_heuristics[index].ApplyHeuristic(currentState, lastUpdatedRow, lastUpdatedCol, _board, _logicHandler, _mrvDict))
                        {
                            metError = true;
                        }
                    }
                }
                // check if the heuristics did anything
                madeProgress = MadeProgress(previousValueChanges, previousPossibilityChanges, currentState);
            }
            return HandleHeuristicsRes(metError, currentState);
        }

        /// <summary>
        /// This func handles the state after applying the heuristics, if needed it resets it.
        /// </summary>
        /// <param name="metError"></param>
        /// <param name="currentState"></param>
        /// <returns>
        /// Returns an enum based on state, invalidState if state was reset, progressMade if the state is valid and there was progress
        /// and noChange if no changes were made
        /// </returns>
        private HeuristicResult HandleHeuristicsRes(bool metError, StateChange currentState)
        {
            if (metError)
            {
                // push the current state object and reset the board
                _stateChangesStack.Push(currentState);
                ResetState();
                return HeuristicResult.InvalidState;
            }
            if (currentState.CellValueChanges.Count > 0 || currentState.CellPossibilityChanges.Count > 0)
            {
                _stateChangesStack.Push(currentState);
                // made progress
                return HeuristicResult.ProgressMade;
            }
            // no progress
            return HeuristicResult.NoChange;
        }

        private bool MadeProgress(int prevValChanges, int prevPossiblityChanges, StateChange currentState)
        {
            return currentState.CellValueChanges.Count > prevValChanges || currentState.CellPossibilityChanges.Count > prevPossiblityChanges;
        }

        /// <summary>
        /// This func attepts to solve the board by placing a value in a cell and recursivly calling the solve func
        /// </summary>
        /// <param name="potentialValue"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool TrySolveCell(int potentialValue, ICell cell, StateChange currentState)
        {
            currentState.CellValueChanges.Add((cell.CellRow, cell.CellCol, 0));
            cell.CellValue = potentialValue;
            // save the affected cell positions incase the attempt is wrong
            HashSet<ICell> affectedCells = _logicHandler.GetFilteredUnitCells(cell.CellRow, cell.CellCol, potentialValue);
            SolverUtils.SetAffectedCellsInStack(currentState, affectedCells);
            SolverUtils.DecreaseGamePossibilites(affectedCells, cell.CellRow, cell.CellCol, potentialValue, _mrvDict, _logicHandler, _board);
            // save the current state
            _stateChangesStack.Push(currentState);
            if (_logicHandler.IsInvalidUpdate(affectedCells))
            {
                // reset the state and add back possibilites
                ResetState();
                return false;
            }
            // Insert the affected cells back into the mrv dict
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
        /// This func resets the game board after a failed guess attempt
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        private void ResetState()
        {
            IEnumerable<ICell> affectedCells = ResetCellsUsingStack();
            _mrvDict.UpdateMRVCells(affectedCells, true);
            _lastUpdatedCell = null;
        }

        /// <summary>
        /// This func will reset the state using the stack and the last state of the board
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ICell> ResetCellsUsingStack()
        {
            StateChange oldState = _stateChangesStack.Pop();
            List<ICell> changedCells = new List<ICell>();
            foreach ((int row, int col, int value) in oldState.CellValueChanges)
            {
                ICell cell = _board[row, col];
                cell.CellValue = value;
                changedCells.Add(cell);
            }
            foreach ((int row, int col, int removedValue) in oldState.CellPossibilityChanges)
            {
                ICell cell = _board[row, col];
                if (!changedCells.Contains(cell))
                {
                    _mrvDict.RemoveCell(cell);
                    changedCells.Add(cell);
                }
                cell.SetCellPossibilities(cell.GetCellPossibilities() | removedValue);
            }
            return changedCells;
        }
    }
}
