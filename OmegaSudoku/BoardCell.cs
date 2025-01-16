﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku
{
    class BoardCell
    {
        /// <summary>
        /// This is the class used to store data of a cell
        /// </summary>
        
        // create non setable properties for x,y and the possibiliets array in a cell
        public int CellX { get; }
        public int CellY { get; }
        public int[] Possibilites { get; }
        public int CellValue { get; set; }

        public BoardCell(int xPos, int yPos, int boardSize, int startingNumber, int cellVal)
        {
            // set the cell x,y and possibilite array
            CellX = xPos;
            CellY = yPos;
            CellValue = cellVal;
            Possibilites = new int[boardSize];
            if (CellValue == 0)
            {
                // create the count arr for the possibilites
                // set the starting possibilites to a board cell
                for (int i = startingNumber; i < boardSize; i++)
                {
                    Possibilites[i] = 1;
                }
                // set the counter of the elements
                Possibilites[0] = boardSize;
            }
        }

        public void DecreasePossibility(int possibiltiyValue)
        {
            Possibilites[possibiltiyValue] = 0;
            // dec the counter
            Possibilites[0] -= 1;
        }

        public int NumberOfPossibilites()
        {
            return Possibilites[0];
        }

        public bool CellIsEmpty()
        {
            return CellValue == 0;
        }
    }
}

