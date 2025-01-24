using System;
using System.Collections.Generic;
using System.Linq;

namespace OmegaSudoku.GameLogic
{
    public class Mrvdict
    {
        /// <summary>
        /// This class holds the mrv dict that is used to get the cell with the minimum amount of possibilites in it
        /// </summary>

        public Dictionary<int, HashSet<(int, int)>> MRVPossibilitiesDict { get; private set; }

        public Mrvdict(int boardSize)
        {
            // Create the dict where a possibility count is the key and a hashset of (x,y) tuples are the values
            MRVPossibilitiesDict = new Dictionary<int, HashSet<(int, int)>>();
            // Init the hashsets inside the array, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                MRVPossibilitiesDict[i] = new HashSet<(int, int)>();
            }
        }

        /// <summary>
        /// This func removes a cell from the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            if(possibilitesNum > 0)
            {
                MRVPossibilitiesDict[possibilitesNum].Remove((cell.CellRow, cell.CellCol));
            }
        }

        /// <summary>
        /// This func inserts a cell into the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void InsertCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            MRVPossibilitiesDict[possibilitesNum].Add((cell.CellRow, cell.CellCol));
        }

        /// <summary>
        /// This func finds the lowest possibility cell
        /// </summary>
        /// <returns>Returns the cells (row, col) pos or (-1, -1) if the array is empty</returns>
        public (int, int) GetLowestPossibilityCell()
        {
            for (int index = 1; index <= MRVPossibilitiesDict.Count; index++)
            {
                if (MRVPossibilitiesDict[index].Count != 0)
                {
                    // get the first cell in the first non empty dict in descending order
                    return MRVPossibilitiesDict[index].First();
                }
            }
            // if the array is empty return (-1 , -1) = board solved
            return (-1, -1);
        }

        public void UpdateMRVCells(IEnumerable<BoardCell> affectedCells, bool isInsert)
        {
            foreach (BoardCell cell in affectedCells)
            {
                if (isInsert)
                {
                    // insert the cell into correct hashset
                    InsertCell(cell);
                }
                else
                {
                    // remove the cell from the mrv array
                    RemoveCell(cell);
                }
            }
        }

        /// <summary>
        /// Func that checks if the dict is empty, this is signed by (-1, -1) tuple
        /// </summary>
        /// <param name="rowColTuple"></param>
        /// <returns></returns>
        public bool IsEmptyMap((int, int) rowColTuple)
        {
            if (rowColTuple.Item1 == -1 && rowColTuple.Item2 == -1)
            {
                return true;
            }
            return false;
        }

        public bool HasSinglePossibiltyCell()
        {
            return MRVPossibilitiesDict[1].Count > 0;
        }
    }
}
