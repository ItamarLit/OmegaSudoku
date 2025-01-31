using OmegaSudoku.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic
{
    public class BoardCell : Icell
    {
        /// <summary>
        /// This is the class used to store data of a cell, the possibilities are stored in bitwise
        /// </summary>
       

        private int _possibilites;

        // amount of bits of possiblites 
        private const int BITS_SIZE = 32;

        private int _cellValue;

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
                // dec the counter
            }
        }

        public void IncreasePossibility(int possibilityValue)
        {
            // This func adds the possibility to the cell
            if ((_possibilites & (1 << possibilityValue)) == 0)
            {
                // set the bit
                _possibilites |= (1 << possibilityValue);
                // inc the counter
            }
        }

        public int NumberOfPossibilites()
        {
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

        public HashSet<int> GetPossibilites()
        {
            // This func gets all the values the cell can be
            HashSet<int> potientialValues = new HashSet<int>();
            for(int i = 1; i < BITS_SIZE; i++)
            {
                if ((HasValue(i)))
                {
                    potientialValues.Add(i);
                }
            }
            return potientialValues;
        }

       
        public void SetCellValue(int value)
        {
            CellValue = value;
        }

        public void SetCellMask(int value)
        {
            _possibilites = value;
        }

        public int GetCellMask()
        {
            return _possibilites;
        }

        public int GetFinalCellValue()
        {
            return (int)Math.Log2(_possibilites);
        }
    }
}

