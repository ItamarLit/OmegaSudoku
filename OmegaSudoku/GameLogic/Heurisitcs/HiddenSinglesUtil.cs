using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HiddenSinglesUtil
    {
        /// <summary>
        /// This class is used as an utilitly class that applys the hidden singles heursitic
        /// </summary>
        public static void ApplyHiddenSingles(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the unit cells 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                Dictionary<int, HashSet<(int, int)>>  possibilityDict = GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
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

        private static Dictionary<int, HashSet<(int, int)>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, BoardCell[,] board, IEnumerable<(int, int)> unitCells)
        {
            Dictionary<int, HashSet<(int, int)>> possibilityDict = new Dictionary<int, HashSet<(int, int)>>();
            foreach ((int unitRow, int unitCol) in unitCells)
            {
                // skip filled cells and the current cell pos
                if (!(unitRow == row && unitCol == col) && board[unitRow, unitCol].CellValue == 0)
                {
                    HashSet<int> possibilities = board[unitRow, unitCol].GetPossibilites();

                    foreach (int value in possibilities)
                    {
                        if (!possibilityDict.ContainsKey(value))
                        {
                            possibilityDict[value] = new HashSet<(int, int)>();
                        }
                        // add the cell to the dict
                        possibilityDict[value].Add((unitRow, unitCol));
                    }
                }

            }
            return possibilityDict;
        }

    }
}
