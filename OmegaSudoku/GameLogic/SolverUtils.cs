using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic
{
    public class SolverUtils
    {
        /// <summary>
        /// This class holds utils that are used to solve the sudoku
        /// </summary>


        /// <summary>
        /// This func sets the changed cells and there possiblities in the stack
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="affectedCells"></param>
        /// <param name="removedPossibility"></param>
        public static void SetAffectedCellsInStack(StateChange currentState, IEnumerable<Icell> affectedCells)
        {
            foreach (Icell cell in affectedCells)
            {
                currentState.CellPossibilityChanges.Add((cell.GetCellRow(), cell.GetCellCol(), cell.GetCellMask()));
            }
        }

        public static void DecreaseGamePossibilites(IEnumerable<Icell> affectedCells, int row, int col,
            int potentialValue, Mrvdict mrvInstance, SudokuLogicHandler logicHandler, Icell[,] board)
        {
            // remove the possibilites
            mrvInstance.UpdateMRVCells(affectedCells, false);
            // remove the current cell
            mrvInstance.RemoveCell(board[row, col]);
            logicHandler.DecreasePossibilites(affectedCells, potentialValue);
        }
    }
}
