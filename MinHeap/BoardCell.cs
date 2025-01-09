using System.Collections.Generic;

namespace OmegaSudoku.MinHeap
{
    class BoardCell
    {
        /// <summary>
        /// This is the class used to store data of a cell, it is used as a heap node aswell
        /// </summary>
        private int CellX { get; }
        private int CellY { get; }
        private HashSet<int> Possibilites { get; }

        private int CellValue { get; set; }

        public BoardCell(int xPos, int yPos, int boardSize, int startingNumber, int cellVal)
        {
            CellX = xPos;
            CellY = yPos;
            CellValue = cellVal;
            if (CellValue == -1)
            {
                // set the starting possibilites to a board cell
                for (int i = startingNumber; i < boardSize; i++)
                {
                    Possibilites.Add(i);
                }
            }
        }

        public void DecreasePossibility(int possibiltiyValue)
        {
            Possibilites.Remove(possibiltiyValue);
        }
        
    }
}
