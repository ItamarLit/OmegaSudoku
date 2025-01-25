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
        /// <summary>
        /// This class is used to apply the heuristic of hidden N sets in sudoku, a hidden set occurs when N cell have N unique possiblites between them
        /// After identifiying this i can remove the other possiblites from the cells turning them into Naked sets
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <param name="logicHandler"></param>
        /// <param name="mrvInstance"></param>
        /// <param name="setSize"></param>
        /// <returns></returns>
        public static bool ApplyHiddenSet(StateChange currentState, int row, int col, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance, int setSize)
        {
            // get the unit cells 
            HashSet<(int, int)>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                // create the possiblity dict
                Dictionary<int, List<(int, int)>> possibilityDict = GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
                List<int> candidates = possibilityDict.Keys.ToList();
                // generate the different sets of the values
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
                        // invalid board there cant be n cells with n + 1 possiblites
                        return false;
                    }
                    else if (setCells.Count == setSize)
                    {
                        // check if the board after removing possiblites is valid
                        finishedSuccessfully = RemoveRedundantPossibilitiesFromSet(logicHandler, setCells, set, mrvInstance, currentState);
                        if (!finishedSuccessfully)
                        {
                            return finishedSuccessfully;
                        }
                    }

                }

            }
            return true;
        }

        private static bool RemoveRedundantPossibilitiesFromSet(SudokuLogicHandler logicHandler, HashSet<(int, int)> setCells, List<int> setCandidates, Mrvdict mrvInstance, StateChange currentState)
        {
            HashSet<BoardCell> trippleCells = logicHandler.GetCellsByIndexes(setCells);
            foreach (BoardCell cell in trippleCells)
            {
                // remove the cell from the dict
                mrvInstance.RemoveCell(cell);
                foreach (int candidate in cell.GetPossibilites().ToList())
                {
                    // remove all the other candidates from the set that dont appear in the combo
                    if (!setCandidates.Contains(candidate))
                    {
                        cell.DecreasePossibility(candidate);
                        currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, candidate));
                    }
                }
                // invalid board state
                if (cell.NumberOfPossibilites() == 0)
                {
                    return false;
                }
                // re add the cell after updating its possiblites
                mrvInstance.InsertCell(cell);

            }

            return true;
        }

        /// <summary>
        /// This func sums the possiblites in a unit
        /// </summary>
        /// <param name="logicHandler"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <param name="unitCells"></param>
        /// <returns></returns>
        private static Dictionary<int, List<(int, int)>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, BoardCell[,] board, IEnumerable<(int, int)> unitCells)
        {
            Dictionary<int, List<(int, int)>> possibilityDict = new Dictionary<int, List<(int, int)>>();
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
                            possibilityDict[value] = new List<(int, int)>();
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