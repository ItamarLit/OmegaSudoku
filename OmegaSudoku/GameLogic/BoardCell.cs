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

        private readonly int _cellRow;
        private readonly int _cellCol;

        private int _possibilites;

        // amount of bits of possiblites 
        private const int BITS_SIZE = 32;

        private int _possibilitiesCount;
        private int _cellValue;

        public BoardCell(int xPos, int yPos, int boardSize, int cellVal)
        {
            _cellRow = xPos;
            _cellCol = yPos;
            _cellValue = cellVal;
            if (_cellValue == 0)
            {
                // create the bit possiblities
                _possibilites = (1 << (boardSize + 1)) - 1;
                _possibilitiesCount = boardSize;
            }
            else
            {
                _possibilites = 0;
                _possibilitiesCount = 0;
            }
        }

        public void DecreasePossibility(int possibiltiyValue)
        {
            // This func removes the possibility to the cell
            if ((_possibilites & (1 << possibiltiyValue)) != 0)
            {
                // turn the bit off using a mask
                _possibilites &= ~(1 << possibiltiyValue); ;
                // dec the counter
                _possibilitiesCount--;
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
                _possibilitiesCount++;
            }
        }

        public int NumberOfPossibilites()
        {
            return _possibilitiesCount;
        }

        public bool IsCellEmpty()
        {
            return _cellValue == 0;
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

        public int GetCellRow()
        {
            return _cellRow;
        }

        public int GetCellCol()
        {
            return _cellCol;
        }

        public int GetCellValue()
        {
            return _cellValue;
        }

        public void SetCellValue(int value)
        {
            _cellValue = value;
        }
    }
}

