using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class PointingPairs : IHeuristic
    {
        /// <summary>
        /// This class is used to apply the pointing pairs heuristic, if 2 cells have 2 possibilites that appear only in them in the block and 
        /// the cells are in the same row / column we can remove all the same possiblity from the cells in the row / column outside the block
        /// </summary>
       
        public bool ApplyHeuristic(StateChange currentState, int row, int col, Icell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // create a possiblity dict for the cube cells
            IEnumerable<Icell> cubeCells = logicHandler.GetCubeCells(row, col);
            Dictionary<int, List<Icell>> possibilityDict = GetPossibilityDict(logicHandler, row, col, board, cubeCells);
            FilterDict(possibilityDict);
            // now we can go over the dict and remove possiblities from the correct unit
            foreach(var kvp in possibilityDict)
            {
                int possiblityVal = kvp.Key;
                if (!RemoveRedundantPossibilites(FindUnitCells(kvp.Value, logicHandler), kvp.Value, mrvInstance, currentState, possiblityVal)) 
                {
                    return false;
                }
            }
            return true;   
        }

        private static Dictionary<int, List<Icell>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, Icell[,] board, IEnumerable<Icell> unitCells)
        {
            Dictionary<int, List<Icell>> possibilityDict = new Dictionary<int, List<Icell>>();
            foreach (var cell in unitCells)
            {
                // skip filled cells and the current cell pos
                if (!(cell.GetCellRow() == row && cell.GetCellCol() == col) && cell.IsCellEmpty())
                {

                    foreach (int value in cell.GetPossibilites())
                    {
                        if (!possibilityDict.ContainsKey(value))
                        {
                            possibilityDict[value] = new List<Icell>();
                        }
                        // add the cell to the dict
                        possibilityDict[value].Add(cell);
                    }
                }

            }
            return possibilityDict;
        }

        private static void FilterDict(Dictionary<int, List<Icell>> possiblityDict)
        {
            // this func removes lists of less than 2 cells from the dict and also removes lists with 2 possiblitys that pairs arent in the same row / col unit
            foreach(var keyValue in possiblityDict)
            {
                if (possiblityDict[keyValue.Key].Count != 2)
                {
                    possiblityDict.Remove(keyValue.Key);
                }
                else
                {
                    // has 2 cells
                    List<Icell> cellList = keyValue.Value;
                    if (!SameRow(cellList[0], cellList[1]) || !SameCol(cellList[0], cellList[1]))
                    {
                        possiblityDict.Remove(keyValue.Key);
                    }
                }
            }
        }

        private static bool SameRow(Icell firstCell, Icell secondCell)
        {
            return firstCell.GetCellRow() == secondCell.GetCellRow();
        }

        private static bool SameCol(Icell firstCell, Icell secondCell)
        {
            return firstCell.GetCellCol() == secondCell.GetCellCol();
        }

        private static bool RemoveRedundantPossibilites(IEnumerable<Icell> unitCells, List<Icell> pointingPair, Mrvdict mrvInstance, StateChange currentState, int possibility)
        {
           
            foreach (Icell cell in unitCells)
            {
                if (cell.IsCellEmpty() && !pointingPair.Contains(cell))
                {
                    mrvInstance.RemoveCell(cell);
                    // remove the possibilites from the cells that arent in the pair
                    
                    if (cell.HasValue(possibility))
                    {
                        cell.DecreasePossibility(possibility);
                        currentState.CellPossibilityChanges.Add((cell.GetCellRow(), cell.GetCellCol(), possibility));
                    }
                    // if a cell has no possiblites then the board is invalid
                    if (cell.NumberOfPossibilites() == 0)
                    {
                        return false;
                    }
                    mrvInstance.InsertCell(cell);
                }

            }
            return true;
        }

        private static IEnumerable<Icell> FindUnitCells(List<Icell> listCells, SudokuLogicHandler logicHandler)
        {
            if (SameRow(listCells[0], listCells[1]))
            {
                return logicHandler.GetRowCells(listCells[0].GetCellRow());
            }
            else
            {
                return logicHandler.GetColumnCells(listCells[0].GetCellCol());
            }
        }

    }
}
