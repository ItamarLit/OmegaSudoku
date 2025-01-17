using System;
using System.Collections.Generic;
using System.Linq;


namespace OmegaSudoku.GameLogic
{
    class SudokuSolver
    {
        /// <summary>
        /// This class holds the main solve func that is used to attempt to solve the board and other helper funcs
        /// </summary>
        
        private readonly BoardCell[,] _board;

        private readonly MrvArray _mrvArray;

        private readonly SudokuLogicHandler _logicHandler;

        public SudokuSolver(BoardCell[,] gameBoard, MrvArray mrvInstance)
        {
            _board = gameBoard;
            _mrvArray = mrvInstance;
            // create the game logic handler
            _logicHandler = new SudokuLogicHandler(_board, _mrvArray);
        }

        public bool Solve()
        {
            // main solve func 
            return false;
        }

        public void ResetBoard() 
        {
          
        }
    }
}
