﻿using System;
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
        public Dictionary<(int, int), BoardCell>[] MRVPossibilitesArray { get; private set; }

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
            MRVPossibilitesArray[possibilitesNum].Remove(tuple, out BoardCell removedCell);
            return removedCell;
        }

        public void InsertCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            
            // Func that inserts a cell into the MrvArray
            MRVPossibilitesArray[possibilitesNum].Add((cell.CellRow, cell.CellCol), cell);
        }

        public BoardCell GetLowestPossibilityCell()
        {
            for (int index = 1; index < MRVPossibilitesArray.Length; index++)
            {
                if (MRVPossibilitesArray[index].Count != 0)
                {
                    // get the first cell in the first non empty hashmap in descending order
                    return MRVPossibilitesArray[index].First().Value;
                }
            }
            // if the array is empty return null = board solved
            return null;
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
