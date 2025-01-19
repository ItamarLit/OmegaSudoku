using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic
{
    class BoardCell
    {
        /// <summary>
        /// This is the class used to store data of a cell
        /// </summary>

        // create non setable properties for x,y and the possibiliets array in a cell
        public int CellRow { get; }
        public int CellCol { get; }
        public int[] Possibilites { get; set; }
        public int CellValue { get; set; }

        public BoardCell(int xPos, int yPos, int boardSize, int startingNumber, int cellVal)
        {
            // set the cell x,y and possibilite array
            CellRow = xPos;
            CellCol = yPos;
            CellValue = cellVal;
            Possibilites = new int[boardSize + 1];
            if (CellValue == 0)
            {
                // create the count arr for the possibilites
                // set the starting possibilites to a board cell
                for (int i = startingNumber; i < Possibilites.Length; i++)
                {
                    Possibilites[i] = 1;
                }
                // set the counter of the elements
                Possibilites[0] = boardSize;
            }
        }

        public void DecreasePossibility(int possibiltiyValue)
        {
            // This func removes the possibility to the cell
            if (Possibilites[possibiltiyValue] != 0) 
            {
                Possibilites[possibiltiyValue] = 0;
                // dec the counter
                Possibilites[0] -= 1;
            }
        }

        public void IncreasePossibility(int possibilityValue)
        {
            // This func adds the possibility to the cell
            if (Possibilites[possibilityValue] == 0)
            {
                Possibilites[possibilityValue] = 1;
                // dec the counter
                Possibilites[0] += 1;
            }
        }

        public int NumberOfPossibilites()
        {
            return Possibilites[0];
        }

        public bool CellIsEmpty()
        {
            return CellValue == 0;
        }

        public bool HasValue(int value)
        {
            return Possibilites[value] != 0;
        }

        public List<int> GetPossibilites()
        {
            // This func gets all the values the cell can be
            List<int> potientialValues = new List<int>();
            for(int i = 1; i < Possibilites.Length; i++)
            {
                if (Possibilites[i] != 0)
                {
                    potientialValues.Add(i);
                }
            }
            return potientialValues;
        }
    }
}

