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
        /// This func sets the changed cells and there possiblities in the stack
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="affectedCells"></param>
        /// <param name="removedPossibility"></param>
        public static void SetAffectedCellsInStack(StateChange currentState, IEnumerable<BoardCell> affectedCells, int removedPossibility)
        {
            foreach (BoardCell cell in affectedCells)
            {
                currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, removedPossibility));
            }
        }

        public static void DecreaseGamePossibilites(IEnumerable<BoardCell> affectedCells, int row, int col,
            int potentialValue, Mrvdict mrvInstance, SudokuLogicHandler logicHandler, BoardCell[,] board)
        {
            // remove the possibilites
            mrvInstance.UpdateMRVCells(affectedCells, false);
            // remove the current cell
            mrvInstance.RemoveCell(board[row, col]);
            logicHandler.DecreasePossibilites(affectedCells, potentialValue);
        }
    }
}
