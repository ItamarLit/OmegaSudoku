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
        public static bool ApplyNakedPairs(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the affected cells from the last change
            HashSet<BoardCell>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                // get all cells with 2 possiblites
                HashSet<BoardCell> pairCells = GetPairPossibilityCells(row, col, board, affectedUnitCells[i]);
                HashSet<(BoardCell, BoardCell)> nakedPairCells = FindNakedPairs(pairCells);
                if (nakedPairCells == null)
                {
                    // board was invalid
                    return false; 
                }
                foreach ((BoardCell firstCell, BoardCell secondCell) pair in nakedPairCells)
                {
                    // get the pairs unit cells that we need to change
                    HashSet<BoardCell> unitCells = GetCorrectUnitCells(pair.firstCell, pair.secondCell, logicHandler, board.GetLength(0));
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

        private static HashSet<(BoardCell, BoardCell)> FindNakedPairs(HashSet<BoardCell> pairCells)
        {
            HashSet<(BoardCell, BoardCell)> nakedPairCells = new HashSet<(BoardCell, BoardCell)>();
            foreach (BoardCell cell in pairCells)
            {
                int pairCount = 0;
                foreach (BoardCell cell2 in pairCells)
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

        private static HashSet<BoardCell> GetCorrectUnitCells(BoardCell firstCell, BoardCell secondCell, SudokuLogicHandler logicHandler, int boardSize)
        {
            HashSet<BoardCell> unitCells = new HashSet<BoardCell>();
            // get the correct unitcells
            if (SameCube(firstCell, secondCell, boardSize))
            {
                unitCells = logicHandler.GetCubeCells(firstCell.CellRow, firstCell.CellCol);
            }
            if (SameRow(firstCell, secondCell))
            {
                unitCells.UnionWith(logicHandler.GetRowCells(firstCell.CellRow));
            }
            if (SameCol(firstCell, secondCell))
            {
                unitCells.UnionWith(logicHandler.GetColumnCells(firstCell.CellCol));
            }
            return unitCells;
        }

        private static bool RemoveRedundantPossibilites(IEnumerable<BoardCell> unitCells, (BoardCell firstCell, BoardCell secondCell) nakedPair, Mrvdict mrvInstance, StateChange currentState)
        {
            HashSet<int> possibilites = nakedPair.Item1.GetPossibilites();
            foreach (BoardCell cell in unitCells) 
            {
                if(cell.CellValue == 0 && !cell.Equals(nakedPair.firstCell) && !cell.Equals(nakedPair.secondCell))
                {
                    mrvInstance.RemoveCell(cell);
                    // remove the possibilites from the cells that arent in the pair
                    foreach (int possibility in possibilites)
                    {
                        if (cell.HasValue(possibility))
                        {
                            cell.DecreasePossibility(possibility);
                            currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, possibility));
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

        private static bool SameCube(BoardCell firstCell, BoardCell secondCell, int boardSize)
        {
            int cubeSize = (int)Math.Sqrt((double)boardSize);
            return (firstCell.CellRow / cubeSize == secondCell.CellRow / cubeSize && firstCell.CellCol / cubeSize == secondCell.CellCol / cubeSize);
        }

        private static bool SameRow(BoardCell firstCell, BoardCell secondCell)
        {
            return firstCell.CellRow == secondCell.CellRow;
        }

        private static bool SameCol(BoardCell firstCell, BoardCell secondCell)
        {
            return firstCell.CellCol == secondCell.CellCol;
        }

        private static HashSet<BoardCell> GetPairPossibilityCells(int row, int col, BoardCell[,] board, IEnumerable<BoardCell> unitCells)
        {
            HashSet<BoardCell> pairPossibilityCells = new HashSet<BoardCell>();
            foreach (var currentCell in unitCells)
            {
                // skip the filled cells and the current cell
                if (!(currentCell.CellRow == row && currentCell.CellCol == col) && currentCell.CellValue == 0)
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
    

