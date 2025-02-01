using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;


namespace OmegaSudoku.GameLogic
{
    public class SolverUtils
    {
        /// <summary>
        /// This class holds utils that are used to solve the sudoku
        /// </summary>


        /// <summary>
        /// This func sets the changed cells and there possibilities bitmask in the stack
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="affectedCells"></param>
        /// <param name="removedPossibility"></param>
        public static void SetAffectedCellsInStack(StateChange currentState, IEnumerable<Icell> affectedCells)
        {
            foreach (Icell cell in affectedCells)
            {
                currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, cell.GetCellPossibilities()));
            }
        }

        /// <summary>
        /// This func is used to decrease the possibilities on the board based on a specific value that was used
        /// </summary>
        /// <param name="affectedCells"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="potentialValue"></param>
        /// <param name="mrvInstance"></param>
        /// <param name="logicHandler"></param>
        /// <param name="board"></param>
        public static void DecreaseGamePossibilites(IEnumerable<Icell> affectedCells, int row, int col,
            int potentialValue, Mrvdict mrvInstance, SudokuLogicHandler logicHandler, Icell[,] board)
        {
            // remove the possibilites
            mrvInstance.UpdateMRVCells(affectedCells, false);
            // remove the current cell
            mrvInstance.RemoveCell(board[row, col]);
            logicHandler.DecreasePossibilites(affectedCells, potentialValue);
        }

        public static int GetCubeIndex(int row, int col, int boardSize)
        {
            int cubeSize = (int)Math.Sqrt(boardSize);
            return (row / cubeSize) * cubeSize + (col / cubeSize);
        }
    }
}
