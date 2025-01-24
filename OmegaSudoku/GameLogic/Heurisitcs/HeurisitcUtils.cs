using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HeurisitcUtils
    {
        public static Dictionary<int, HashSet<(int, int)>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, BoardCell[,] board, IEnumerable<(int, int)> unitCells)
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


        public static HashSet<BoardCell> GetPairPossibilityCells(int row, int col, BoardCell[,] board, IEnumerable<(int, int)> unitCells)
        {
            HashSet<BoardCell> pairPossibilityCells = new HashSet<BoardCell>();
            foreach ((int unitRow, int unitCol) in unitCells)
            {
                BoardCell currentCell = board[unitRow, unitCol];
                // skip the filled cells and the current cell
                if (!(unitRow == row && unitCol == col) && currentCell.CellValue == 0)
                {
                    if(currentCell.GetPossibilites().Count == 2)
                    {
                        pairPossibilityCells.Add(currentCell);
                    }
                }

            }
            return pairPossibilityCells;
        }
    }
}
