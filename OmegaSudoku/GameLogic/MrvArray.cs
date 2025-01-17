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
            if(possibilitesNum != 0)
            {
                (int x, int y) tuple = (cell.CellRow, cell.CellCol);
                // Func that removes a cell from the MrvArray
                MRVPossibilitesArray[possibilitesNum].Remove(tuple);
            }
          
        }

        public bool InsertCell(BoardCell cell)
        {
            // this func will return a bool value so i can know if there is a cell that has no values
            int possibilitesNum = cell.NumberOfPossibilites();
            if(cell.CellValue == 0)
            {
                if(possibilitesNum == 0)
                {
                    return false;
                }
                // Func that inserts a cell into the MrvArray
                MRVPossibilitesArray[possibilitesNum].Add((cell.CellRow, cell.CellCol));
            }
            return true;
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


        public void PrintArray()
        {
            // print func used for debugging
            for (int index = 1; index < MRVPossibilitesArray.Length; index++)
            {
                Console.WriteLine($"Possibility number: {index}, amount of cells: {MRVPossibilitesArray[index].Count}");
            }
           
        }
    }
}
