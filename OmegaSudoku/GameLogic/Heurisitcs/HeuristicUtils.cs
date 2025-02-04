using OmegaSudoku.Interfaces;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HeuristicUtils
    {
        // This class is used as a util class, it holds important functions that all heuristics need.


        /// <summary>
        /// This func is used to remove possibilities from the board efficiantly using bitmasks
        /// </summary>
        /// <param name="unitCells"></param>
        /// <param name="excludedCells"></param>
        /// <param name="setCandidates"></param>
        /// <param name="excludedCandidates"></param>
        /// <param name="mrvInstance"></param>
        /// <param name="currentState"></param>
        /// <returns>The func returns true if the changes it made to the board didn't make it invalid, else it returns false</returns>
        public static bool RemoveRedundantPossibilities(IEnumerable<ICell> unitCells, IEnumerable<ICell> excludedCells, int setCandidates, int excludedCandidates, Mrvdict mrvInstance, StateChange currentState)
        {
            // create the mask for the candidates that need to be removed from the unitcells
            int candidatesToRemove = (setCandidates & ~excludedCandidates) + 1;
            foreach (ICell cell in unitCells)
            {
                // go over each empty cell in the unit cells that shouldn't be excluded
                if (cell.IsCellEmpty() && (excludedCells == null || !excludedCells.Contains(cell)))
                {
                    // remove the cell from the mrv dict
                    mrvInstance.RemoveCell(cell);
                    int oldMask = cell.GetCellPossibilities();
                    // create the newMask for the cell by removing the candidates
                    int newMask = oldMask & ~candidatesToRemove;
                    if (newMask != oldMask)
                    {
                        // set the new mask in the cell
                        cell.SetCellPossibilities(newMask);
                        // save the old mask in the changes
                        currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, oldMask));
                    }
                    // check if a cell has 0 possibilites left (invalid board)
                    if (cell.NumberOfPossibilites() == 0)
                    {
                        return false;
                    }
                    // reinsert the newly changed cell into the mrvDict
                    mrvInstance.InsertCell(cell);
                }
            }
            // the board is valid after the changes
            return true;
        }

    }
}
