using System;
using OmegaSudoku.MinHeap;

namespace OmegaSudoku.IO
{
    class InputHandler
    {
        /// <summary>
        /// This is the class used to handle all the input functions
        /// the class will check to see that the board is valid and then the board can be attempted to solve
        /// </summary>

        private static string Input { get; set; }

        public static void GetInput()
        {
            // get the users input
            Input = Console.ReadLine();
        }

        public static BoardCell[,] CheckInput()
        {
            int validSudokuSize = 9;
            int startingSudokuNum = 0;
            // check for only numeric values
            if(!double.TryParse(Input, out double boardValue))
            {
                // Need to throw an exception here
                Console.WriteLine("Invalid board given");
                return null;
            }
            // check sudoku rules for valid board
            int boardSize = (int)Math.Sqrt((double)Input.Length);
            if (boardSize != validSudokuSize)
            {
                // Need to throw an exception here
                Console.WriteLine("Invalid board size entered");
                return null;
            }
            // setup the board
            BoardCell[,] board = new BoardCell[boardSize, boardSize];
            for(int index = 0; index < Input.Length; index++)
            {
                // convert char into int
                int cellValue = Input[index] - '0';
                int rowIndex = index / validSudokuSize;
                int columnIndex = index % validSudokuSize;
                // set the boardCell
                // the validSudokuSize and startingSudokuNum are for setting the number range on the board ( setting the possible values in a cell)
                board[rowIndex, columnIndex] = new BoardCell(rowIndex, columnIndex, validSudokuSize, startingSudokuNum, cellValue);
            }
            return board;
        }

       

    }
}
