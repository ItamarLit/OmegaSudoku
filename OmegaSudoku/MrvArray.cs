using System;
using System.Collections.Generic;

namespace OmegaSudoku
{
    class MrvArray
    {
        /// <summary>
        /// This class holds the mrv array that is used to get the cell with the minimum amount of possibilites in it
        /// </summary>

        // Create an Arra
        private Dictionary<(int, int), BoardCell>[] MRVPossibilitesArray { get ; set; }

        public MrvArray(int boardSize)
        {
            // Create the array of hashmaps where a (x,y) tuple is the key and a BoardCell is the value
            MRVPossibilitesArray = new Dictionary<(int, int), BoardCell>[boardSize + 1];
            // Init the hashmaps inside the array, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                MRVPossibilitesArray[i] = new Dictionary<(int, int), BoardCell>();
            }
        }

        public BoardCell RemoveCell(int possibilitesNum, (int x, int y) tuple)
        {
            // Func that removes a cell from the MrvArray
            return null;
        }

        public void InsertCell(int possibilitesNum, (int x, int y) tuple, BoardCell cell)
        {
            // Func that inserts a cell into the MrvArray
        }

    }
}
