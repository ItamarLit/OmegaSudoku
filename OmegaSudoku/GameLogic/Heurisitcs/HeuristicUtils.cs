using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HeuristicUtils
    {
        public static bool RemoveRedundantPossibilities(IEnumerable<Icell> unitCells, IEnumerable<Icell> excludedCells, int setCandidates, int excludedCandidates, Mrvdict mrvInstance, StateChange currentState)
        {
            // create the mask for the candidates that need to be removed from the unitcells
            int candidatesToRemove = (setCandidates & ~excludedCandidates) + 1;
            foreach (Icell cell in unitCells)
            {
                if (cell.IsCellEmpty() && (excludedCells == null || !excludedCells.Contains(cell)))
                {
                    // remove the cell from the mrv dict
                    mrvInstance.RemoveCell(cell);
                    int oldMask = cell.GetCellMask();
                    // create the newMask for the cell by removing the candidates
                    int newMask = oldMask & ~candidatesToRemove;
                    if (newMask != oldMask)
                    {
                        // set the new mask in the cell
                        cell.SetCellMask(newMask);
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

            return true;
        }

    }
}
