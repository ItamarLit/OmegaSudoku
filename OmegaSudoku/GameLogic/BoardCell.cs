using OmegaSudoku.Interfaces;
using System;
using System.Collections;


namespace OmegaSudoku.GameLogic
{
    public class BoardCell : Icell
    {
        /// <summary>
        /// This is the class used to store data of a cell, the possibilities are stored in a bitmask
        /// </summary>
       
        private int _possibilites;

        // properties from the interface
        public int CellCol { get; set; }
        public int CellRow { get; set; }
        public int CellValue { get ; set ; }

        public BoardCell(int xPos, int yPos, int boardSize, int cellVal)
        {
            CellRow = xPos;
            CellCol = yPos;
            CellValue = cellVal;
            if (CellValue == 0)
            {
                // create the bit possiblities
                _possibilites = (1 << (boardSize + 1)) - 2;
            }
            else
            {
                _possibilites = 0;
            }
        }

        public void DecreasePossibility(int possibiltiyValue)
        {
            // This func removes the possibility to the cell
            if ((_possibilites & (1 << possibiltiyValue)) != 0)
            {
                // turn the bit off using a mask
                _possibilites &= ~(1 << possibiltiyValue); 
            }
        }

        public void IncreasePossibility(int possibilityValue)
        {
            // This func adds the possibility to the cell
            if ((_possibilites & (1 << possibilityValue)) == 0)
            {
                // set the bit
                _possibilites |= (1 << possibilityValue);
            }
        }

        public int NumberOfPossibilites()
        {
            // this func gets the number of possibilities a cell has
            int count = 0;
            int bits = _possibilites;
            while (bits > 0)
            {
                // clear lowest bit
                bits &= (bits - 1);
                count++;
            }
            return count;
        }

        public bool IsCellEmpty()
        {
            return CellValue == 0;
        }

        public bool HasValue(int value)
        {
            return (_possibilites & (1 << value)) != 0;
        }

        public List<int> GetPossibilites()
        {
            // This func gets all the values the cell can be in int and not bitmask
            List<int> potientialValues = new List<int>();
            int value = 1;
            int tempMask = _possibilites;
            tempMask >>= 1;
            while (tempMask > 0)
            {
                if((tempMask & 1) == 1)
                {
                    potientialValues.Add(value);
                }
                tempMask >>= 1;
                value++;
            }
            return potientialValues;
        }

        public void SetCellPossibilities(int value)
        {
            _possibilites = value;
        }

        public int GetCellPossibilities()
        {
            return _possibilites;
        }

        public int GetFinalCellValue()
        {
            return (int)Math.Log2(_possibilites);
        }


    }
}

