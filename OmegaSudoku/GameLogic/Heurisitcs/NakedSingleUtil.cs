using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;


namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSingleUtil
    {
        /// <summary>
        /// This class is used as a utilitly class that applys the naked singles heursitic a naked single is a cell that has only one possiblity
        /// if we find a cell with 1 possibility this must be its value, it is not implemented using the nakedSets class because this is the only heuristic that changes
        /// A cells value.
        /// </summary>

        /// <summary>
        /// This func is used to apply the naked singles heuristic to the board
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="board"></param>
        /// <param name="mrvInstance"></param>
        /// <param name="lastUpdatedCell"></param>
        /// <param name="logicHandler"></param>
        /// <returns>The func returns false if the board is left in an invalid state after running else, it returns true</returns>
        public static bool SolveSinglePossibilityCells(StateChange currentState, Icell[,] board, Mrvdict mrvInstance, ref Icell? lastUpdatedCell, SudokuLogicHandler logicHandler)
        {
            // run while there are naked singles on the board
            while (mrvInstance.HasSinglePossibiltyCell())
            {
                // get the row, col of the cell
                Icell cell = mrvInstance.GetLowestPossibilityCell(logicHandler, board);
                // get the cells value
                int potentialValue = cell.GetFinalCellValue();
                // change the value and save the change
                currentState.CellValueChanges.Add((cell.CellRow, cell.CellCol, cell.CellValue));
                cell.CellValue = potentialValue;
                // remove the possibility
                HashSet<Icell> affectedCells = logicHandler.GetFilteredUnitCells(cell.CellRow, cell.CellCol, potentialValue);
                SolverUtils.SetAffectedCellsInStack(currentState, affectedCells);
                SolverUtils.DecreaseGamePossibilites(affectedCells, cell.CellRow, cell.CellCol, potentialValue, mrvInstance, logicHandler, board);
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
