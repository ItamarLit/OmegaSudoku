using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HeurisitcUtils
    {
        public static Dictionary<int, HashSet<(int, int)>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, BoardCell[,] board, HashSet<(int, int)> unitCells)
        {
            Dictionary<int, HashSet<(int, int)>> possibilityDict = new Dictionary<int, HashSet<(int, int)>>();
            foreach ((int unitRow, int unitCol) in unitCells)
            {
                if (!(unitRow == row && unitCol == col) && board[unitRow, unitCol].CellValue == 0)
                {
                    HashSet<int> possibilities = board[unitRow, unitCol].GetPossibilites();

                    foreach (int value in possibilities)
                    {
                        if (!possibilityDict.ContainsKey(value))
                        {
                            possibilityDict[value] = new HashSet<(int, int)>();
                        }

                        possibilityDict[value].Add((unitRow, unitCol));
                    }
                }

            }
            return possibilityDict;


        }
    }
}
