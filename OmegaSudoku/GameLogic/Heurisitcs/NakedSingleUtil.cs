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
       
        public static bool SolveSinglePossibilityCells(StateChange currentState, BoardCell[,] board, Mrvdict mrvInstance, ref BoardCell? lastUpdatedCell, SudokuLogicHandler logicHandler)
        {
            // run while there are naked singles on the board
            while (mrvInstance.HasSinglePossibiltyCell())
            {
                // get the row, col of the cell
                BoardCell cell = mrvInstance.GetLowestPossibilityCell(logicHandler, board);
                int potentialValue = cell.GetPossibilites().First();
                currentState.CellValueChanges.Add((cell.CellRow, cell.CellCol, cell.CellValue));
                cell.CellValue = potentialValue;
                // remove the possibility
                HashSet<BoardCell> affectedCells = logicHandler.GetFilteredUnitCells(cell.CellRow, cell.CellCol, potentialValue);
                SolverUtils.SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
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
