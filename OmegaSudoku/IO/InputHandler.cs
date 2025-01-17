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

        // set the valid size for the board
        private const int VALID_SUDOKU_SIZE = 9;
        // set the starting number for the sudoku to 1 ( 1 - 9 )
        private const int STARTING_SUDOKU_NUMBER = 1;
        public static void GetUserInput()
        {
            // get the users input
            _input = Console.ReadLine();
        }

        public static void CheckInput()
        {
            // check for only numeric values
            if (!double.TryParse(_input, out double boardValue))
            {
                // Throw invalid board info exception
                throw new BoardInfoException();
            }
            // check sudoku rules for valid board
            int boardSize = (int)Math.Sqrt((double)_input.Length);
            if (boardSize != VALID_SUDOKU_SIZE)
            {
                // Throw invalid board size exception
                throw new BoardSizeException(boardSize);
            }
        }

        public static BoardCell[,] SetUpBoard()
        {
            // setup the board
            BoardCell[,] board = new BoardCell[VALID_SUDOKU_SIZE, VALID_SUDOKU_SIZE];
            int cellValue;
            int rowIndex;
            int columnIndex;
            for (int index = 0; index < _input.Length; index++)
            {
                // convert char into int
                cellValue = _input[index] - '0';
                rowIndex = index / VALID_SUDOKU_SIZE;
                columnIndex = index % VALID_SUDOKU_SIZE;
                // check the cellValue
                if (cellValue > VALID_SUDOKU_SIZE || cellValue < STARTING_SUDOKU_NUMBER - 1)
                {
                    // Throw cell info exception
                    throw new CellInfoExeption(cellValue);
                }
                // the ValidSudokuSize and StartingSudokuNumber are for setting the number range on the board ( setting the possible values in a cell)
                board[rowIndex, columnIndex] = new BoardCell(rowIndex, columnIndex, VALID_SUDOKU_SIZE, STARTING_SUDOKU_NUMBER, cellValue);
            }
            return board;
        }


    }
}