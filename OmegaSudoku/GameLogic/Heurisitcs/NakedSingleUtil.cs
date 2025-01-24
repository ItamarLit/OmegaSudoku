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
        /// This class is used as a utilitly class that applys the naked singles heursitic
        /// </summary>
       
        public static bool SolveSinglePossibilityCells(StateChange currentState, BoardCell[,] board, Mrvdict mrvInstance, ref (int, int)? lastUpdatedCell, SudokuLogicHandler logicHandler)
        {
            // run while there are naked singles on the board
            while (mrvInstance.HasSinglePossibiltyCell())
            {
                // get the row, col of the cell
                (int row, int col) = mrvInstance.GetLowestPossibilityCell();
                int potentialValue = board[row, col].GetPossibilites().First();
                currentState.CellValueChanges.Add((row, col, board[row, col].CellValue));
                board[row, col].CellValue = potentialValue;
                // remove the possibility
                HashSet<BoardCell> affectedCells = logicHandler.GetFilteredUnitCells(row, col, potentialValue);
                SolverUtils.SetAffectedCellsInStack(currentState, affectedCells, potentialValue);
                SolverUtils.DecreaseGamePossibilites(affectedCells, row, col, potentialValue, mrvInstance, logicHandler, board);
                // check if the update was valid
                if (logicHandler.IsInvalidUpdate(affectedCells))
                {
                    return false;
                }
                lastUpdatedCell = (row, col);
                mrvInstance.UpdateMRVCells(affectedCells, true);
            }
            return true;
        }
    }
}
