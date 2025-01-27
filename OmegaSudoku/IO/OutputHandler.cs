using OmegaSudoku.Interfaces;
using System;


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

        public static void PrintBoard(Icell[,] board)
        {
            int boardSize = board.GetLength(0);
            int blockSize = (int)Math.Sqrt((double)boardSize);
            Console.WriteLine("Here is your board:");

            string line = new string(' ', 1) + new string('-', (blockSize * 3 + 1) * (boardSize / blockSize) - 1);
            Console.WriteLine(line);
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (j % blockSize == 0)
                    {
                        Console.Write("|");
                    }

                    char cellChar = (char)(board[i, j].GetCellValue() + '0');
                    string cellValue = cellChar.ToString();
                    Console.Write($" {cellValue} ");
                }
                Console.Write("|");
                Console.WriteLine();

                if ((i + 1) % blockSize == 0)
                {
                    Console.WriteLine(line); 
                }
            }
        }

    }
}