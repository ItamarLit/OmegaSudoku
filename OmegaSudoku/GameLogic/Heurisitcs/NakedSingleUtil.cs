using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSingleUtil
    {
        /// <summary>
        /// This class is used as a utilitly class that applys the naked singles heursitic a naked single is a cell that has only one possiblity
        /// if we find a cell with 1 possibility this must be its value
        /// </summary>
       
        public static bool SolveSinglePossibilityCells(StateChange currentState, Icell[,] board, Mrvdict mrvInstance, ref Icell? lastUpdatedCell, SudokuLogicHandler logicHandler)
        {
            // run while there are naked singles on the board
            while (mrvInstance.HasSinglePossibiltyCell())
            {
                // get the row, col of the cell
                Icell cell = mrvInstance.GetLowestPossibilityCell(logicHandler, board);
                int potentialValue = cell.GetPossibilites().First();
                currentState.CellValueChanges.Add((cell.GetCellRow(), cell.GetCellCol(), cell.GetCellValue()));
                cell.SetCellValue(potentialValue);
                // remove the possibility
                HashSet<Icell> affectedCells = logicHandler.GetFilteredUnitCells(cell.GetCellRow(), cell.GetCellCol(), potentialValue);
                SolverUtils.SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
                SolverUtils.DecreaseGamePossibilites(affectedCells, cell.GetCellRow(), cell.GetCellCol(), potentialValue, mrvInstance, logicHandler, board);
                // check if the update was valid
                if (logicHandler.IsInvalidUpdate(affectedCells))
                {
                    return false;
                }
                lastUpdatedCell = cell;
                mrvInstance.UpdateMRVCells(affectedCells, true);
            }
            return true;
        }
    }
}
