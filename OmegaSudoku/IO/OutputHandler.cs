using OmegaSudoku.Interfaces;
using System;
using System.Text;


namespace OmegaSudoku.IO
{
    class OutputHandler
    {
        /// <summary>
        /// This is the class used to handle all the output functions to the user
        /// the class will check to see that the board is valid and then the board can be attempted to solve
        /// </summary>

        public static void RequestBoard()
        {
            Console.WriteLine("Please enter the sudoku board: ");
        }

        public static void ShowImpossibleBoardMsg()
        {
            Console.WriteLine("The entered board is impossible to solve");
        }

        public static void ShowProgramRuntime(long elapsedTime)
        {
            Console.WriteLine($"The program ran for: {elapsedTime} milliseconds");
        }

        public static void OutputError(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void ShowMenu()
        {
            Console.WriteLine("Here are your options:\n");
            Console.WriteLine("*** To solve a board entered in the console enter: 'solve_c'");
            Console.WriteLine("*** To solve a board entered in a file enter: 'solve_f'");
            Console.WriteLine("*** To see the solver rules enter: 'rules'");
            Console.WriteLine("*** To see this menu again enter: 'menu'");
            Console.WriteLine("*** To exit the solver enter: 'exit'");
        }

        public static void ShowRules()
        {
            Console.WriteLine("A valid board for the solver is an N*N board where N can be squared and is smaller / equal to 25.");
        }

        public static string BuildStringForBoard(Icell[,] board)
        {
            int boardSize = board.GetLength(0);
            int blockSize = (int)Math.Sqrt((double)boardSize);
            string line = new string(' ', 1) + new string('-', (blockSize * 3 + 1) * (boardSize / blockSize) - 1);
            StringBuilder str = new StringBuilder();
            str.AppendLine(line);
            for (int row = 0; row < board.GetLength(0); row++) 
            { 
                for(int col = 0; col < board.GetLength(1); col++)
                {
                    if (col % blockSize == 0)
                    {
                        str.Append("|");
                    }
                    char cellChar = (char)(board[row, col].CellValue + '0');
                    str.Append(" " + cellChar + " ");
                }
                str.Append("|");
                str.AppendLine();
                if ((row + 1) % blockSize == 0)
                {
                    str.AppendLine(line);
                }
            }
            return str.ToString();
        }
        public static void PrintBoard(Icell[,] board)
        {
            Console.WriteLine("Here is your board:");
            string boardStr = BuildStringForBoard(board);
            Console.WriteLine(boardStr);
        }
        public static string GetBoardStr(Icell[,] board)
        {
            string boardStr = "";
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int j = 0; j < board.GetLength(1); j++)
                {
                    boardStr += (char)(board[i, j].CellValue + '0');
                }
            }
            return boardStr;
        }

        public static void WriteIntoFile(string filePath, string outputStr)
        {
            File.AppendAllText(filePath, "\n\nHere is the output:\n\n");
            File.AppendAllText(filePath, outputStr);
        }
    }
}