using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;


namespace OmegaSudoku.GameLogic
{
    public class Mrvdict
    {
        /// <summary>
        /// This class holds the mrv dict that is used to track cells and the amount of possibilities they have.
        /// </summary>

        private Dictionary<int, HashSet<ICell>> _MRVPossibilitiesDict { get;  }
        // flag for small boards
        private bool _isSmallBoard;
        public Mrvdict(int boardSize)
        {
            // Create the dict where a possibility count is the key and a hashset of cells are the values
            _MRVPossibilitiesDict = new Dictionary<int, HashSet<ICell>>();
            // Init the hashsets inside the dict, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                _MRVPossibilitiesDict[i] = new HashSet<ICell>();
            }
            if(boardSize <= 9)
            {
                _isSmallBoard = true;
            }
            else
            {
                _isSmallBoard = false;
            }
        }

        /// <summary>
        /// This func removes a cell from the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveCell(ICell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            if(possibilitesNum > 0)
            {
                _MRVPossibilitiesDict[possibilitesNum].Remove(cell);
            }
        }

        /// <summary>
        /// This func inserts a cell into the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void InsertCell(ICell cell)
        {
            _MRVPossibilitiesDict[cell.NumberOfPossibilites()].Add(cell);
        }

        /// <summary>
        /// This func finds the lowest possibility cell, if the board is smaller / equal to 9 * 9 then
        /// it is also the cell in the most filled part of the board, if the board is larger than 9 * 9 it doesn't help to get the cell
        /// in the most filled part (the board is to big)
        /// </summary>
        /// <returns>Returns the cell or null if the dict is empty</returns>
        public ICell GetLowestPossibilityCell(SudokuLogicHandler logicHandler, ICell[,] board)
        {
            for (int index = 1; index <= _MRVPossibilitiesDict.Count; index++)
            {
                if (_MRVPossibilitiesDict[index].Count != 0)
                {
                    // check if the board is big, if it is its not worth running the GetBestCell on (it doesnt help but rathers makes the runtime worse)
                    if (_isSmallBoard)
                    {
                        // get the best cell between the lowest possiblites 
                        return GetBestCell(_MRVPossibilitiesDict[index], board, logicHandler);
                    }
                    else
                    {
                        // get the first cell from the cells with the least values
                        return _MRVPossibilitiesDict[index].First();
                    }    
                }
            }
            // if the array is empty return null = board solved
            return null;
        }


        /// <summary>
        /// This func gets the cell that is in the most filled area and has the lowest possiblity count
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="board"></param>
        /// <param name="logicHandler"></param>
        /// <returns>This func returns the cell in the most filled area with the lowest possibility count</returns>
        private ICell GetBestCell(HashSet<ICell> cells, ICell[,] board, SudokuLogicHandler logicHandler)
        {
            ICell bestCell = null;
            int maxSize = board.GetLength(0) + 1;
            // set the counts to max size so every other count is smaller
            int bestRowEmptyCount = maxSize;
            int bestColEmptyCount = maxSize;
            int bestBoxEmptyCount = maxSize;

            foreach (var cell in cells)
            {
                // get the count values
                int rowCount = logicHandler.CountEmptyNeighbours(logicHandler.GetRowCells(cell.CellRow));
                int colCount = logicHandler.CountEmptyNeighbours(logicHandler.GetColumnCells(cell.CellCol));
                int cubeCount = logicHandler.CountEmptyNeighbours(logicHandler.GetCubeCells(cell.CellRow, cell.CellCol));
                // check first by row, then by col then by cube
                if (rowCount < bestRowEmptyCount ||
                    (rowCount == bestRowEmptyCount && colCount < bestColEmptyCount) ||
                    (rowCount == bestRowEmptyCount && colCount == bestColEmptyCount && cubeCount < bestBoxEmptyCount))
                {
                    bestCell = cell;
                    bestRowEmptyCount = rowCount;
                    bestColEmptyCount = colCount;
                    bestBoxEmptyCount = cubeCount;
                }
            }
            return bestCell;
        }

        public void UpdateMRVCells(IEnumerable<ICell> affectedCells, bool isInsert)
        {
            foreach (ICell cell in affectedCells)
            {
                if (isInsert)
                {
                    // insert the cell into correct hashset
                    InsertCell(cell);
                }
                else
                {
                    // remove the cell from the mrv dict
                    RemoveCell(cell);
                }
            }
        }

        /// <summary>
        /// Func that checks if the dict is empty, this is signed by null
        /// </summary>
        /// <param name="rowColTuple"></param>
        /// <returns>Returns true if the dict is empty else false</returns>
        public bool IsEmptyMap(ICell cell)
        {
            if (cell == null)
            {
                return true;
            }
            return false;
        }

        public bool HasSinglePossibiltyCell()
        {
            return _MRVPossibilitiesDict[1].Count > 0;
        }

        public HashSet<ICell> GetCellsWithPossibilites(int numPossiblites)
        {
            return _MRVPossibilitiesDict[numPossiblites];
        }
        
    }
}
