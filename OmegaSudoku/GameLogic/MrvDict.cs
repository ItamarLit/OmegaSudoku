using System;
using System.Collections.Generic;
using System.Linq;

namespace OmegaSudoku.GameLogic
{
    public class Mrvdict
    {
        /// <summary>
        /// This class holds the mrv dict that is used to get the cell with the minimum amount of possibilites in it
        /// </summary>

        private Dictionary<int, HashSet<(int, int)>> _MRVPossibilitiesDict { get;  }

        public Mrvdict(int boardSize)
        {
            // Create the dict where a possibility count is the key and a hashset of (x,y) tuples are the values
            _MRVPossibilitiesDict = new Dictionary<int, HashSet<(int, int)>>();
            // Init the hashsets inside the array, cell 1 will represent cells with only one possibility and so on
            for (int i = 1; i < boardSize + 1; i++)
            {
                _MRVPossibilitiesDict[i] = new HashSet<(int, int)>();
            }
        }

        /// <summary>
        /// This func removes a cell from the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void RemoveCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            if(possibilitesNum > 0)
            {
                _MRVPossibilitiesDict[possibilitesNum].Remove((cell.CellRow, cell.CellCol));
            }
        }

        /// <summary>
        /// This func inserts a cell into the dict based on the cells possibilites
        /// </summary>
        /// <param name="cell"></param>
        public void InsertCell(BoardCell cell)
        {
            int possibilitesNum = cell.NumberOfPossibilites();
            _MRVPossibilitiesDict[possibilitesNum].Add((cell.CellRow, cell.CellCol));
        }

        /// <summary>
        /// This func finds the lowest possibility cell in the most filled part of the board
        /// </summary>
        /// <returns>Returns the cells (row, col) pos or (-1, -1) if the array is empty</returns>
        public (int, int) GetLowestPossibilityCell(SudokuLogicHandler logicHandler, BoardCell[,] board)
        {
            for (int index = 1; index <= _MRVPossibilitiesDict.Count; index++)
            {
                if (_MRVPossibilitiesDict[index].Count != 0)
                {
                    // get the best cell between the lowest possiblites 
                    return GetBestCell(_MRVPossibilitiesDict[index], board, logicHandler);
                }
            }
            // if the array is empty return (-1 , -1) = board solved
            return (-1, -1);
        }

        /// <summary>
        /// This func gets the cell that is in the most filled area and has the lowes possiblity count
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="board"></param>
        /// <param name="logicHandler"></param>
        /// <returns></returns>
        private (int, int) GetBestCell(HashSet<(int, int)> cells, BoardCell[,] board, SudokuLogicHandler logicHandler)
        {
            (int row, int col) bestCell = (-1, -1);
            // set the counts to max value so every other count is smaller
            int bestRowEmptyCount = int.MaxValue;
            int bestColEmptyCount = int.MaxValue;
            int bestBoxEmptyCount = int.MaxValue;

            foreach (var cell in cells)
            {
                // get the count values
                int rowCount = logicHandler.CountEmptyNeighbours(logicHandler.GetRowCells(cell.Item1));
                int colCount = logicHandler.CountEmptyNeighbours(logicHandler.GetColumnCells(cell.Item2));
                int cubeCount = logicHandler.CountEmptyNeighbours(logicHandler.GetCubeCells(cell.Item1, cell.Item2));
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

        public void UpdateMRVCells(IEnumerable<BoardCell> affectedCells, bool isInsert)
        {
            foreach (BoardCell cell in affectedCells)
            {
                if (isInsert)
                {
                    // insert the cell into correct hashset
                    InsertCell(cell);
                }
                else
                {
                    // remove the cell from the mrv array
                    RemoveCell(cell);
                }
            }
        }

        /// <summary>
        /// Func that checks if the dict is empty, this is signed by (-1, -1) tuple
        /// </summary>
        /// <param name="rowColTuple"></param>
        /// <returns></returns>
        public bool IsEmptyMap((int, int) rowColTuple)
        {
            if (rowColTuple.Item1 == -1 && rowColTuple.Item2 == -1)
            {
                return true;
            }
            return false;
        }

        public bool HasSinglePossibiltyCell()
        {
            return _MRVPossibilitiesDict[1].Count > 0;
        }

        public HashSet<(int, int)> GetCellsWithPossibilites(int numPossiblites)
        {
            return _MRVPossibilitiesDict[numPossiblites];
        }
        
    }
}
