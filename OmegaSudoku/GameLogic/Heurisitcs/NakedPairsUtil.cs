using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedPairsUtil
    {
        public static bool ApplyNakedPairs(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // This func will apply the hidden singles heuristic to the board 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                HashSet<BoardCell> pairCells = HeurisitcUtils.GetPairPossibilityCells(row, col, board, affectedUnitCells[i]);
                HashSet<(BoardCell, BoardCell)> nakedPairCells = new HashSet<(BoardCell, BoardCell)>();
                foreach (BoardCell cell in pairCells) 
                { 
                    foreach (BoardCell cell2 in pairCells)
                    {
                        // check if the cell is the same instance
                        if(!cell.Equals(cell2) && CheckPair(cell.GetPossibilites(), cell2.GetPossibilites()))
                        {
                            // add one cell to know which possibilites to remove 
                            nakedPairCells.Add((cell, cell2));
                            // exit the outer loop
                            break;
                        }
                    }
                }
                foreach((BoardCell, BoardCell) pair in nakedPairCells)
                {
                    HashSet<BoardCell> unitCells = logicHandler.GetCellsByIndexes(affectedUnitCells[i]);
                    bool wasValid = RemoveRedundantPossibilites(unitCells, pair, mrvInstance, currentState);
                    if (!wasValid)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool RemoveRedundantPossibilites(IEnumerable<BoardCell> unitCells, (BoardCell, BoardCell) nakedPair, Mrvdict mrvInstance, StateChange currentState)
        {
            HashSet<int> possibilites = nakedPair.Item1.GetPossibilites();
            foreach (BoardCell cell in unitCells) 
            {
                if(cell.CellValue == 0)
                {
                    mrvInstance.RemoveCell(cell);
                    // remove the possibilites from the cells that arent in the pair
                    if (!cell.Equals(nakedPair.Item1) && !cell.Equals(nakedPair.Item2))
                    {
                        foreach (int possibility in possibilites)
                        {
                            if (cell.HasValue(possibility))
                            {
                                cell.DecreasePossibility(possibility);
                                currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, possibility));
                            }
                        }
                    }
                    if (cell.GetPossibilites().Count == 0)
                    {
                        return false;
                    }
                    mrvInstance.InsertCell(cell);
                }
               
            }
            return true;
        }
        private static bool CheckPair(IEnumerable<int> firstSet, IEnumerable<int> secondSet) 
        {
            bool areSameFlag = true;
            foreach (int i in firstSet) 
            {
                if (!secondSet.Contains(i))
                {
                    areSameFlag = false; 
                    break;
                }
            }
            return areSameFlag;
        }
    }
}
