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

        /// <summary>
        /// Because searching for a hidden set is costly, i only search for them in the units that the row, col cell is in.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <param name="logicHandler"></param>
        /// <param name="mrvInstance"></param>
        /// <returns>The func returns false if the board is in an invalid state after running else it returns true</returns>
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
                        finishedSuccessfully = HeuristicUtils.RemoveRedundantPossibilities(setCells, null, CreateFullSetBit(board.GetLength(0)), CreateSetMask(set), mrvInstance, currentState);
                        // check if the board after removing possiblites is valid
                        if (!finishedSuccessfully)
                        {
                            return finishedSuccessfully;
                        }
                    }

                }

            }
            return true;
        }

 

        /// <summary>
        /// This func sums the possiblites in a unit and sets them in a dict
        /// </summary>
        /// <param name="logicHandler"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <param name="unitCells"></param>
        /// <returns>A dict of possibility - cells pairs where each possibility is linked to the cells that have it</returns>
        private static Dictionary<int, List<Icell>> GetPossibilityDict(SudokuLogicHandler logicHandler, int row, int col, Icell[,] board, IEnumerable<Icell> unitCells)
        {
            Dictionary<int, List<Icell>> possibilityDict = new Dictionary<int, List<Icell>>();
            foreach (var cell in unitCells)
            {
                // skip filled cells and the current cell pos
                if (!(cell.CellRow == row && cell.CellCol == col) && cell.IsCellEmpty())
                {
                    int cellMask = cell.GetCellMask();
                    // skip the first bit as it is never set
                    cellMask >>= 1;
                    int value = 1;
                    // go over the mask and add the possibilites
                    while (value <= board.GetLength(0) && cellMask != 0)
                    {
                        if ((cellMask & 1) != 0)
                        {
                            if (!possibilityDict.TryGetValue(value, out var list))
                            {
                                list = new List<Icell>();
                                possibilityDict[value] = list;
                            }
                            list.Add(cell);

                        }
                        cellMask >>= 1;
                        value++;
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

        /// <summary>
        /// Thi func creates a bit mask for a set of numbers in a list
        /// </summary>
        /// <param name="set"></param>
        /// <returns>Bitmask for the numbers</returns>
        private static int CreateSetMask(List<int> set)
        {
            int mask = 0;
            foreach (int val in set)
            {
                mask |= 1 << val;
            }
            return mask;
        }

        /// <summary>
        /// This func creates a fully set bitmask for a given board size
        /// A board of size 9 -> 1111111110, each bit symbols a valid possibility, bit 0 is always off
        /// </summary>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        public static int CreateFullSetBit(int boardSize)
        {
            return (1 << boardSize + 1) - 2;
        }

    }
}