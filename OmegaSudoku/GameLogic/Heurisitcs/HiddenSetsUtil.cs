using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HiddenSetsUtil
    {
        public static bool ApplyHiddenSet(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance, int setSize)
        {
            // get the unit cells 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                // create the possiblity dict
                Dictionary<int, HashSet<(int, int)>> possibilityDict = GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
                List<int> candidates = possibilityDict.Keys.ToList();
                // generate the different triples of the values
                List<List<int>> sets = GetCombinationsInSet(candidates, setSize);
                bool finishedSuccessfully = false;
                foreach (var set in sets)
                {
                    HashSet<(int, int)> setCells = new HashSet<(int, int)>();
                    foreach (int value in set)
                    {
                        setCells.UnionWith(possibilityDict[value]);
                    }
                    if (setCells.Count < setSize)
                    {
                        // invalid board there cant be 2 cells with only 3 possiblites
                        return false;
                    }
                    else if (setCells.Count == setSize)
                    {
                        // found a hidden tripple
                        finishedSuccessfully =  RemoveRedundantPossibilitiesFromSet(logicHandler, setCells, set, mrvInstance, currentState);
                        if (!finishedSuccessfully)
                        {
                            return finishedSuccessfully;
                        }
                    }

                }

            }
            return true;
        }

        private static bool RemoveRedundantPossibilitiesFromSet(SudokuLogicHandler logicHandler, HashSet<(int, int)> tripleCells, List<int> tripleCandidates, Mrvdict mrvInstance, StateChange currentState)
        {
            HashSet<BoardCell> trippleCells = logicHandler.GetCellsByIndexes(tripleCells);
            foreach (BoardCell cell in trippleCells)
            {

                mrvInstance.RemoveCell(cell);

                foreach (int candidate in cell.GetPossibilites().ToList())
                {
                    if (!tripleCandidates.Contains(candidate))
                    {
                        cell.DecreasePossibility(candidate);
                        currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, candidate));
                    }
                }
                if (cell.GetPossibilites().Count == 0)
                {
                    return false;
                }
                mrvInstance.InsertCell(cell);

            }

            return true;
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

        /// <summary>
        /// Recursive function that generates sets of n len from a list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static List<List<int>> GetCombinationsInSet(List<int> list, int length)
        {
            
            List<List<int>> result = new List<List<int>>();
            // base case of the recursion
            if (length == 0)
            {
                result.Add(new List<int>());
                return result;
            }

            for (int i = 0; i < list.Count; i++)
            {
                List<int> remaining = list.Skip(i + 1).ToList();
                List<List<int>> combinations = GetCombinationsInSet(remaining, length - 1);

                foreach (var combination in combinations)
                {
                    combination.Insert(0, list[i]);
                    result.Add(combination);
                }
            }

            return result;
        }

    }
}