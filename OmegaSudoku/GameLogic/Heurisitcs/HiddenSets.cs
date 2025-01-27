using OmegaSudoku.Interfaces;
using OmegaSudoku.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class HiddenSets : IHeuristic
    {
        /// <summary>
        /// This class is used to apply the heuristic of hidden N sets in sudoku, a hidden set occurs when N cell have N unique possiblites between them
        /// After identifiying this i can remove the other possiblites from the cells turning them into Naked sets
        /// </summary>
        /// 
        private int _setSize;
        public HiddenSets(int setSize) 
        {
            _setSize = setSize;
        }

        public bool ApplyHeuristic(StateChange currentState, int row, int col, Icell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the unit cells 
            IEnumerable<Icell>[] affectedUnitCells = { logicHandler.GetRowCells(row), logicHandler.GetColumnCells(col), logicHandler.GetCubeCells(row, col) };
            for (int i = 0; i < affectedUnitCells.Length; i++)
            {
                // create the possiblity dict
                Dictionary<int, List<Icell>> possibilityDict = GetPossibilityDict(logicHandler, row, col, board, affectedUnitCells[i]);
                List<int> candidates = possibilityDict.Keys.ToList();
                // generate the different sets of the values
                List<List<int>> sets = GetCombinationsInSet(candidates, _setSize);
                bool finishedSuccessfully = false;
                foreach (var set in sets)
                {
                    HashSet<Icell> setCells = new HashSet<Icell>();
                    foreach (int value in set)
                    {
                        setCells.UnionWith(possibilityDict[value]);
                    }
                    if (setCells.Count < _setSize)
                    {
                        // invalid board there cant be n cells with n + 1 possiblites
                        return false;
                    }
                    else if (setCells.Count == _setSize)
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

        private static bool RemoveRedundantPossibilitiesFromSet(SudokuLogicHandler logicHandler, HashSet<Icell> setCells, List<int> setCandidates, Mrvdict mrvInstance, StateChange currentState)
        {
            foreach (Icell cell in setCells)
            {
                // remove the cell from the dict
                mrvInstance.RemoveCell(cell);
                foreach (int candidate in cell.GetPossibilites().ToList())
                {
                    // remove all the other candidates from the set that dont appear in the combo
                    if (!setCandidates.Contains(candidate))
                    {
                        cell.DecreasePossibility(candidate);
                        currentState.CellPossibilityChanges.Add((cell.GetCellRow(), cell.GetCellCol(), candidate));
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

        /// <summary>
        ///  Function that generates sets of n len from a list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static List<List<int>> GetCombinationsInSet(List<int> list, int length)
        {
            List<List<int>> result = new List<List<int>>();
            GenerateCombinations(list, length, 0, new List<int>(), result);
            return result;
        }

        private static void GenerateCombinations(List<int> list, int length, int start, List<int> current, List<List<int>> result)
        {
            if (length == 0)
            {
                result.Add(new List<int>(current));
                return;
            }
            for (int i = start; i < list.Count; i++)
            {
                current.Add(list[i]);
                GenerateCombinations(list, length - 1, i + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }

    }
}