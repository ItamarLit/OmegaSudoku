using OmegaSudoku.Exceptions;
using OmegaSudoku.GameLogic;
using OmegaSudoku.Interfaces;


namespace OmegaSudoku.IO
{
    public class InputValidator
    {
        /// <summary>
        /// This class is used to validate the user input
        /// </summary>

        // set the starting number for the sudoku to 1 
        private const int STARTING_SUDOKU_NUMBER = 1;

        private const int MAX_BOARD_SIZE = 25;

        public static bool CheckInput(string input)
        {
            if(input == "")
            {
                throw new EmptyInputException(); 
            }
            // This func will check the basic input
            double boardSize = Math.Sqrt((double)input.Length);
            // check if the board size is N * N where N is able to be squared
            if (boardSize != Math.Floor(boardSize) || boardSize > MAX_BOARD_SIZE)
            {
                // throw invalid board size exception
                throw new BoardSizeException(input.Length);
            }
            // no exception so the size is valid
            return true;
        }

        public static ICell[,] SetUpBoard(string input)
        {
            int boardSize = (int)Math.Sqrt((double)input.Length);
            // setup the board
            BoardCell[,] board = new BoardCell[boardSize, boardSize];
            int cellValue;
            int rowIndex;
            int columnIndex;
            for (int index = 0; index < input.Length; index++)
            {
                // convert char into int
                cellValue = input[index] - '0';
                rowIndex = index / boardSize;
                columnIndex = index % boardSize;
                // check the cellValue
                int num = boardSize;
                if (cellValue > boardSize || cellValue < STARTING_SUDOKU_NUMBER - 1)
                {
                    // Throw cell info exception
                    throw new CellInfoExeption((char)(cellValue + '0'));
                }
                // create the board cell
                board[rowIndex, columnIndex] = new BoardCell(rowIndex, columnIndex, boardSize, cellValue);
            }
            return board;
        }
    }
}
