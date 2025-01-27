using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedPairsUtil
    {
        /// <summary>
        /// This class is used as a utilitly class that applys the naked pairs heursitic, a naked pair is a set of 2 cells that have 2 same possiblites between them
        /// after finding a pair we can remove the rest of the possiblites from the unit cells
        /// </summary>
        public static bool ApplyNakedPairs(StateChange currentState, int row, int col, Icell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the affected cells from the last change
            IEnumerable<Icell>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                // get all cells with 2 possiblites
                HashSet<(Icell, Icell)> nakedPairCells = FindNakedPairs(GetPairPossibilityCells(row, col, board, affectedUnitCells[i]));
                if (nakedPairCells == null)
                {
                    // board was invalid
                    return false; 
                }
                foreach ((Icell firstCell, Icell secondCell) pair in nakedPairCells)
                {
                    // get the pairs unit cells that we need to change
                    HashSet<Icell> unitCells = GetCorrectUnitCells(pair.firstCell, pair.secondCell, logicHandler, board.GetLength(0));
                    // remove the possiblites from the unit
                    bool wasValid = RemoveRedundantPossibilites(unitCells, pair, mrvInstance, currentState);
                    if (!wasValid)
                    {
                        // board was invalid
                        return false;
                    }
                }
            }
            return true;
        }

        private static HashSet<(Icell, Icell)> FindNakedPairs(HashSet<Icell> pairCells)
        {
            HashSet<(Icell, Icell)> nakedPairCells = new HashSet<(Icell, Icell)>();
            foreach (Icell cell in pairCells)
            {
                int pairCount = 0;
                foreach (Icell cell2 in pairCells)
                {
                    // check if the cell is the same instance
                    if (!cell.Equals(cell2) && CheckPair(cell.GetPossibilites(), cell2.GetPossibilites()))
                    {
                        // add one cell to know which possibilites to remove 
                        nakedPairCells.Add((cell, cell2));
                        pairCount++;
                    }
                }
                // the board cant have more than 2 cells with the exact same possiblites and only those
                if (pairCount > 1)
                {
                    return null;
                }
            }
            return nakedPairCells;
        }

        private static HashSet<Icell> GetCorrectUnitCells(Icell firstCell, Icell secondCell, SudokuLogicHandler logicHandler, int boardSize)
        {
            HashSet<Icell> unitCells = new HashSet<Icell>();
            // get the correct unitcells
            if (SameCube(firstCell, secondCell, boardSize))
            {
                unitCells = logicHandler.GetCubeCells(firstCell.GetCellRow(), firstCell.GetCellCol()).ToHashSet();
            }
            if (SameRow(firstCell, secondCell))
            {
                unitCells.UnionWith(logicHandler.GetRowCells(firstCell.GetCellRow()));
            }
            if (SameCol(firstCell, secondCell))
            {
                unitCells.UnionWith(logicHandler.GetColumnCells(firstCell.GetCellCol()));
            }
            return unitCells;
        }

        private static bool RemoveRedundantPossibilites(IEnumerable<Icell> unitCells, (Icell firstCell, Icell secondCell) nakedPair, Mrvdict mrvInstance, StateChange currentState)
        {
            foreach (Icell cell in unitCells) 
            {
                if(cell.GetCellValue() == 0 && !cell.Equals(nakedPair.firstCell) && !cell.Equals(nakedPair.secondCell))
                {
                    mrvInstance.RemoveCell(cell);
                    // remove the possibilites from the cells that arent in the pair
                    foreach (int possibility in nakedPair.Item1.GetPossibilites())
                    {
                        if (cell.HasValue(possibility))
                        {
                            cell.DecreasePossibility(possibility);
                            currentState.CellPossibilityChanges.Add((cell.GetCellRow(), cell.GetCellCol(), possibility));
                        }
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
        private static bool CheckPair(IEnumerable<int> firstSet, IEnumerable<int> secondSet)
        {
            return firstSet.ToHashSet().SetEquals(secondSet);
        }

        private static bool SameCube(Icell firstCell, Icell secondCell, int boardSize)
        {
            int cubeSize = (int)Math.Sqrt((double)boardSize);
            return (firstCell.GetCellRow() / cubeSize == secondCell.GetCellRow() / cubeSize && firstCell.GetCellCol() / cubeSize == secondCell.GetCellCol() / cubeSize);
        }

        private static bool SameRow(Icell firstCell, Icell secondCell)
        {
            return firstCell.GetCellRow() == secondCell.GetCellRow();
        }

        private static bool SameCol(Icell firstCell, Icell secondCell)
        {
            return firstCell.GetCellCol() == secondCell.GetCellCol();
        }

        private static HashSet<Icell> GetPairPossibilityCells(int row, int col, Icell[,] board, IEnumerable<Icell> unitCells)
        {
            HashSet<Icell> pairPossibilityCells = new HashSet<Icell>();
            foreach (var currentCell in unitCells)
            {
                // skip the filled cells and the current cell
                if (!(currentCell.GetCellRow() == row && currentCell.GetCellCol() == col) && currentCell.GetCellValue() == 0)
                {
                    if (currentCell.NumberOfPossibilites() == 2)
                    {
                        pairPossibilityCells.Add(currentCell);
                    }
                }

            }
            return pairPossibilityCells;
        }
    }
}
    

