using System;

namespace OmegaSudoku.IO
{
    class InputHandler
    {
        /// <summary>
        /// This is the class used to handle all the input functions
        /// the class will check to see that the board is valid and then the board can be attempted to solve
        /// </summary>

        private static string Input { get; set; }
        // set the valid size for the board
        private const int ValidSudokuSize = 9;
        // set the starting number for the sudoku to 1 ( 1 - 9 )
        private const int StartingSudokuNumber = 1;
        public static void GetInput()
        {
            // get the users input
            Input = Console.ReadLine();
        }

        public static void CheckInput()
        {
            // check for only numeric values
            if (!double.TryParse(Input, out double boardValue))
            {
                // Need to throw an exception here
                Console.WriteLine("Invalid board given");
            }
            // check sudoku rules for valid board
            int boardSize = (int)Math.Sqrt((double)Input.Length);
            if (boardSize != ValidSudokuSize)
            {
                // Need to throw an exception here
                Console.WriteLine("Invalid board size entered");
            }
        }

        public static BoardCell[,] SetUpBoard()
        {
            // setup the board
            BoardCell[,] board = new BoardCell[ValidSudokuSize, ValidSudokuSize];
            int cellValue;
            int rowIndex;
            int columnIndex;
            for (int index = 0; index < Input.Length; index++)
            {
                // convert char into int
                cellValue = Input[index] - '0';
                rowIndex = index / ValidSudokuSize;
                columnIndex = index % ValidSudokuSize;
                // check the cellValue
                if (cellValue > ValidSudokuSize || cellValue < StartingSudokuNumber - 1)
                {
                    // Need to throw an exception here
                    Console.WriteLine($"Invalid cell value entered: {cellValue}");
                }
                // the ValidSudokuSize and StartingSudokuNumber are for setting the number range on the board ( setting the possible values in a cell)
                board[rowIndex, columnIndex] = new BoardCell(rowIndex, columnIndex, ValidSudokuSize, StartingSudokuNumber, cellValue);
            }
            return board;
        }


    }
}