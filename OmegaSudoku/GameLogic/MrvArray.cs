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
        public List<(int, int)>[] MRVPossibilitesArray { get; private set; }

        public MrvArray(int boardSize)
        {
            // Create the array of hashmaps where a (x,y) tuple is the key and a BoardCell is the value
            MRVPossibilitesArray = new List<(int, int)>[boardSize + 1];
            // Init the hashmaps inside the array, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                MRVPossibilitesArray[i] = new List<(int, int)>();
            }
        }

        public void RemoveCell(BoardCell cell)
        {
            // this func is called after removing a possibility
            int possibilitesNum = cell.NumberOfPossibilites();
            (int x, int y) tuple = (cell.CellRow, cell.CellCol);
            // Func that removes a cell from the MrvArray
            if(possibilitesNum != 0)
            {
                MRVPossibilitesArray[possibilitesNum].Remove(tuple);

            }
        }

        public void InsertCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            Console.WriteLine(possibilitesNum);
            if(possibilitesNum != 0)
            {
                MRVPossibilitesArray[possibilitesNum].Add((cell.CellRow, cell.CellCol));

            }
        }

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

        public void RemoveAffectedMRVCells(List<BoardCell> affectedCells)
        {
            // This func removes all the affected cells from the MRVarray
            foreach (BoardCell cell in affectedCells)
            {
                // remove the cell from the mrv array
                RemoveCell(cell);

            }
        }

        public void InsertAffectedMRVCells(List<BoardCell> affectedCells)
        {
            // This func inserts all the affected cells pos from the MRVarray
            foreach (BoardCell cell in affectedCells)
            {
                // remove the cell from the mrv array
                InsertCell(cell);

            }
        }
    }
}
