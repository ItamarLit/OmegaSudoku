using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HiddenSinglesUtil
    {
        public static void ApplyHiddenSingles(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the unit cells 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                Dictionary<int, HashSet<(int, int)>>  possibilityDict = HeurisitcUtils.GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
                foreach (var (possibilityValue, possibilityCells) in possibilityDict)
                {
                    // found a hidden single
                    if (possibilityCells.Count == 1)
                    {
                        ApplyHiddenSingle(possibilityValue, possibilityCells.First(), board, mrvInstance, currentState);
                    }
                }
            }
        }

        private static void ApplyHiddenSingle(int possibilityValue, (int , int ) cellPos, BoardCell[,] board, Mrvdict mrvInstance, StateChange currentState)
        {
            (int hiddenSingleRow, int hiddenSingleCol) = cellPos;
            BoardCell hiddenSingleCell = board[hiddenSingleRow, hiddenSingleCol];
            if (hiddenSingleCell.GetPossibilites().Count != 1)
            {
                mrvInstance.RemoveCell(hiddenSingleCell);
                // remove the rest of the possibilites from the cell
                HashSet<int> cellPossibilites = hiddenSingleCell.GetPossibilites();
                foreach (int possiblity in cellPossibilites)
                {
                    if (possiblity != possibilityValue)
                    {
                        currentState.CellPossibilityChanges.Add((hiddenSingleRow, hiddenSingleCol, possiblity));
                        hiddenSingleCell.DecreasePossibility(possiblity);
                    }

                }
                // reinsert the cell with only one possiblity
                mrvInstance.InsertCell(hiddenSingleCell);
            }
        }

    }
}
