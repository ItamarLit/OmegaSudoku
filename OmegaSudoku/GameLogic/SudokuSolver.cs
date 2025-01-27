using System;
using System.Collections.Generic;
using System.Linq;
using OmegaSudoku.GameLogic.Heurisitcs;
using OmegaSudoku.Interfaces;
using OmegaSudoku.IO;

namespace OmegaSudoku.GameLogic
{
    public class SudokuSolver
    {
        /// <summary>
        /// This class holds the main solve func that is used to attempt to solve the board and other helper funcs
        /// </summary>

        private readonly Icell[,] _board;

        private readonly Mrvdict _mrvDict;

        private readonly SudokuLogicHandler _logicHandler;

        private readonly Stack<StateChange> _stateChangesStack;

        private readonly List<IHeuristic> _heuristics = new List<IHeuristic>
        {
            // hidden single and hidden pair
            new HiddenSets(1),
            new HiddenSets(2),
            // naked pair
            new NakedPairs(),
            // pointing pair
            new PointingPairs()
        };

        private Icell? _lastUpdatedCell;

        public static int depth = 0;

        public SudokuSolver(Icell[,] gameBoard, Mrvdict mrvInstance)
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
            HeuristicResult heuristicRes = ApplyHeuristics();
            if(heuristicRes == HeuristicResult.InvalidState)
            {
                // invalid board 
                return false;
            }
            Icell cell = _mrvDict.GetLowestPossibilityCell(_logicHandler, _board);
            if (_mrvDict.IsEmptyMap(cell))
            {
                // if there are no more cells to fill the sudoku is solved
                return true;
            }
            IEnumerable<int> possibilites = SortByLeastConstraining(cell.GetPossibilites(), cell.GetCellRow(), cell.GetCellCol());
            if (possibilites.Count() > 0)
            {
                foreach (int potentialValue in possibilites)
                {
                    _lastUpdatedCell = cell;
                    StateChange currentState = new StateChange();
                    if (TrySolveCell(potentialValue, cell.GetCellRow(), cell.GetCellCol(), currentState))
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
                    int lastUpdatedRow = _lastUpdatedCell.GetCellRow();
                    int lastUpdatedCol = _lastUpdatedCell.GetCellCol();
                    for (int index = 0; index < _heuristics.Count && !metError; index++) 
                    {
                        if (!_heuristics[index].ApplyHeuristic(currentState, lastUpdatedRow, lastUpdatedCol, _board, _logicHandler, _mrvDict))
                        {
                            metError = true;
                        }
                    }
                }
                // check if the heuristics did anything
                madeProgress = MadeProgress(previousValueChanges, previousPossibilityChanges, currentState);
            }
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
            _board[row, col].SetCellValue(potentialValue);
            // save the affected cell positions incase the attempt is wrong
            HashSet<Icell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, potentialValue);
            SolverUtils.SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
            SolverUtils.DecreaseGamePossibilites(affectedCells, row, col, potentialValue, _mrvDict, _logicHandler, _board);
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
        /// This func resets the game board after a failed backtracking attempt
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        private void ResetState()
        {
            IEnumerable<Icell> affectedCells = ResetCellsUsingStack();
            _mrvDict.UpdateMRVCells(affectedCells, true);
            _lastUpdatedCell = null;
        }

        /// <summary>
        /// This func will reset the state using the stack and the last state of the board
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Icell> ResetCellsUsingStack()
        {
            StateChange oldState = _stateChangesStack.Pop();
            List<Icell> changedCells = new List<Icell>();
            foreach ((int row, int col, int value) in oldState.CellValueChanges)
            {
                Icell cell = _board[row, col];
                cell.SetCellValue(value);
                changedCells.Add(cell);
            }
            foreach ((int row, int col, int removedValue) in oldState.CellPossibilityChanges)
            {
                Icell cell = _board[row, col];
                if (!changedCells.Contains(cell))
                {
                    _mrvDict.RemoveCell(cell);
                    changedCells.Add(cell);
                }
                cell.IncreasePossibility(removedValue);
            }
            return changedCells;
        }

        private IEnumerable<int> SortByLeastConstraining(IEnumerable<int> possiblites, int row, int col)
        {
            // only apply this if the board is larger than 9 * 9 as the overhead is not worth it in smaller boards
            if(_board.GetLength(0) <= 9)
            {
                return possiblites;
            }
            Dictionary<int, int> sortDict = new Dictionary<int, int>();
            foreach(int possiblity in possiblites)
            {
                HashSet<Icell> affectedCells = _logicHandler.GetFilteredUnitCells(row, col, possiblity);
                sortDict[possiblity] = affectedCells.Count;
            }
            return sortDict.OrderBy(keyValue =>  keyValue.Value).Select(keyValue => keyValue.Key);

        }
    }
}
