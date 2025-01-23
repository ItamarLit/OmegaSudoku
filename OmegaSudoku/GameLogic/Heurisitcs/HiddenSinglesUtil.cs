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
            // This func will apply the hidden singles heuristic to the board 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                Dictionary<int, HashSet<(int, int)>>  possibilityDict = HeurisitcUtils.GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
                foreach (var dictCell in possibilityDict)
                {
                    int possibilityValue = dictCell.Key;
                    HashSet<(int, int)> possibilityCells = dictCell.Value;
                    // found a hidden single
                    if (possibilityCells.Count == 1)
                    {
                        (int hiddenSingleRow, int hiddenSingleCol) = possibilityCells.First();
                        BoardCell hiddenSingleCell = board[hiddenSingleRow, hiddenSingleCol];
                        if(hiddenSingleCell.GetPossibilites().Count != 1)
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
        }

    }
}
