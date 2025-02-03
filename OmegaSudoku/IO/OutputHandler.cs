using OmegaSudoku.Interfaces;
using System;
using System.Text;


namespace OmegaSudoku.IO
{
    class OutputHandler
    {
        /// <summary>
        /// This is the class used to handle all the output functions to the user, including output to files
        /// </summary>

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
            Console.WriteLine();
            Console.WriteLine("A valid board for the solver is an N*N board where N can be squared and is smaller / equal to 25.");
            Console.WriteLine("When writing an option there is no case sensitivity (solve_C) is valid like (solve_c)");
            Console.WriteLine("The solver runtime is from the start of the solve algorithim to the end, not including the prints");
            Console.WriteLine("Paths for files with \" in the start and end are accepted.");
            Console.WriteLine("If you chose solve_f the output will be in the same file path as the entered path.");
            Console.WriteLine("Any errors in the input will apear only on console, even if input from file");
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

        /// <summary>
        /// This func is used to create an output string for file and console
        /// </summary>
        /// <param name="solved"></param>
        /// <param name="board"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        public static string CreateOutput(bool solved, Icell[,] board, long elapsedTime)
        {
            StringBuilder str = new StringBuilder();
            // get the board as a board ie in columns and rows form
            string boardStr = BuildStringForBoard(board);
            // get the board in str form ( a string of signs)
            string boardInStr = GetBoardStr(board);
            // create the full output str
            if (solved)
            {
                str.AppendLine("\nHere is your solved board:");

            }
            else
            {
                str.AppendLine("\nHere is your board:");
            }
            str.AppendLine(boardStr);
            str.AppendLine("Here is the board in string form:");
            str.AppendLine(boardInStr);
            if (!solved)
            {
                str.AppendLine("----------------------------");
                str.AppendLine("Sorry, this board isn't solvable");
            }
            str.AppendLine("----------------------------");
            str.AppendLine($"The program ran for: {elapsedTime} milliseconds");
            return str.ToString();
        }
        public static void ShowOutput(Icell[,] board, bool solved, long time)
        {
            string outputStr = CreateOutput(solved, board, time);
            Console.WriteLine(outputStr);
        }
        public static string GetBoardStr(Icell[,] board)
        {
            // func that gets the board in a string format
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

        public static void WriteIntoFile(string filePath, Icell[,] board, bool solved, long time)
        {
            // func that writes to a file
            string outputStr = CreateOutput(solved, board, time);
            File.AppendAllText(filePath, outputStr);
        }
    }
}