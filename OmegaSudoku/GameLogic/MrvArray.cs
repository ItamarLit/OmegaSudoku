using System;
using System.Collections.Generic;
using System.Linq;

namespace OmegaSudoku.GameLogic
{
    class MrvArray
    {
        /// <summary>
        /// This class holds the mrv array that is used to get the cell with the minimum amount of possibilites in it
        /// </summary>

        // Create an Array property
        public HashSet<(int, int)>[] MRVPossibilitesArray { get; private set; }

        public MrvArray(int boardSize)
        {
            // Create the array of hashmaps where a (x,y) tuple is the key and a BoardCell is the value
            MRVPossibilitesArray = new HashSet<(int, int)>[boardSize + 1];
            // Init the hashsets inside the array, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                MRVPossibilitesArray[i] = new HashSet<(int, int)>();
            }
        }

        /// <summary>
        /// This func removes a cell from the mrvArray based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveCell(BoardCell cell)
        {
            // this func is called after removing a possibility
            int possibilitesNum = cell.NumberOfPossibilites();
            (int x, int y) tuple = (cell.CellRow, cell.CellCol);
            // Func that removes a cell from the MrvArray
            MRVPossibilitesArray[possibilitesNum].Remove(tuple);    
        }

        /// <summary>
        /// This func inserts a cell into the mrvArray based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void InsertCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            MRVPossibilitesArray[possibilitesNum].Add((cell.CellRow, cell.CellCol));
        }

        /// <summary>
        /// This func finds the lowest possibility cell
        /// </summary>
        /// <returns>Returns the cells row, col pos or -1, -1 if the array is empty</returns>
        public (int, int) GetLowestPossibilityCell()
        {
            for (int index = 1; index < MRVPossibilitesArray.Length; index++)
            {
                if (MRVPossibilitesArray[index].Count != 0)
                {
                    // get the first cell in the first non empty hashmap in descending order
                    return MRVPossibilitesArray[index].First();
                }
            }
            // if the array is empty return (-1 , -1) = board solved
            return (-1, -1);
        }

        public void RemoveAffectedMRVCells(HashSet<BoardCell> affectedCells)
        {
            // This func removes all the affected cells from the MRVarray
            foreach (BoardCell cell in affectedCells)
            {
                // remove the cell from the mrv array
                RemoveCell(cell);

            }
        }

        public void InsertAffectedMRVCells(HashSet<BoardCell> affectedCells)
        {
            // This func inserts all the affected cells pos from the MRVarray
            foreach (BoardCell cell in affectedCells)
            {
                // remove the cell from the mrv array
                InsertCell(cell);

            }
        }

        /// <summary>
        /// Func that checks if the mrvArray is empty, this is signed by (-1, -1) tuple
        /// </summary>
        /// <param name="rowColTuple"></param>
        /// <returns></returns>
        public bool IsEmptyArray((int, int) rowColTuple)
        {
            if (rowColTuple.Item1 == -1 && rowColTuple.Item2 == -1)
            {
                return true;
            }
            return false;
        }
    }
}
