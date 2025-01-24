﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSingleUtil
    {
        public static int SolveSinglePossibilityCells(StateChange currentState, BoardCell[,] board, Mrvdict mrvInstance, ref (int, int)? lastUpdatedCell, SudokuLogicHandler logicHandler)
        {
            int singleCellsAddedCount = 0;
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
                    return -1;
                }
                lastUpdatedCell = (row, col);
                mrvInstance.UpdateMRVCells(affectedCells, true);
                singleCellsAddedCount++;
            }
            return singleCellsAddedCount;
        }
    }
}
