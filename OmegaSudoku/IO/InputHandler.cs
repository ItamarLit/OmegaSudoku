using OmegaSudoku.GameLogic;
using System;
using OmegaSudoku.Exceptions;

namespace OmegaSudoku.IO
{
    class InputHandler
    {
        /// <summary>
        /// This is the class used to handle all the input functions
        /// the class will check to see that the board is valid and then the board can be attempted to solve
        /// </summary>

        private static string _input;

        // set the starting number for the sudoku to 1 
        private const int STARTING_SUDOKU_NUMBER = 1;

        private const int MAX_BOARD_SIZE = 25;

        private static int _boardSize;

        public static void GetUserInput()
        {
            // get the users input
            _input = Console.ReadLine();
        }

        public static bool CheckInput()
        {
            // This func will check the basic input
            if (_input == "EXIT") 
            {
                return false;
            }
            // check for only numeric values
            if (!double.TryParse(_input, out double boardValue))
            {
                // Throw invalid board info exception
                throw new BoardInfoException();
            }
            double boardSize = Math.Sqrt((double)_input.Length);
            // check if the board size is N * N where N is able to be squared
            if (boardSize != Math.Floor(boardSize) || boardSize > MAX_BOARD_SIZE)
            {
                // throw invalid board size exception
                throw new BoardSizeException(_input.Length);
            }
            // no exception so the size is valid
            _boardSize = (int)boardSize;
            return true;
        }

        public static BoardCell[,] SetUpBoard()
        {
            // setup the board
            BoardCell[,] board = new BoardCell[_boardSize, _boardSize];
            int cellValue;
            int rowIndex;
            int columnIndex;
            for (int index = 0; index < _input.Length; index++)
            {
                // convert char into int
                cellValue = _input[index] - '0';
                rowIndex = index / _boardSize;
                columnIndex = index % _boardSize;
                // check the cellValue
                if (cellValue > _boardSize || cellValue < STARTING_SUDOKU_NUMBER - 1)
                {
                    // Throw cell info exception
                    throw new CellInfoExeption(cellValue);
                }
                // the ValidSudokuSize and StartingSudokuNumber are for setting the number range on the board ( setting the possible values in a cell)
                board[rowIndex, columnIndex] = new BoardCell(rowIndex, columnIndex, _boardSize, cellValue);
            }
            return board;
        }


    }
}