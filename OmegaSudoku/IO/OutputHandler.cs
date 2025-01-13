﻿using System;


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

        public static void PrintBoard(BoardCell[,] board)
        {
            Console.WriteLine("Here is your board:");
            Console.WriteLine("-------------");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (j % 3 == 0)
                    {
                        Console.Write("|");
                    }
                    Console.Write(board[i, j].CellValue);
                }
                Console.Write("|");
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine("-------------");
                }
            }
        }

        public static void PrintBoardPossibilites(BoardCell[,] board)
        {
            // used for debugging
            Console.WriteLine("Board Possibilites count:");
            Console.WriteLine("-------------");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (j % 3 == 0)
                    {
                        Console.Write("|");
                    }
                    Console.Write(board[i, j].NumberOfPossibilites());
                }
                Console.Write("|");
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine("-------------");
                }
            }
        }
    }
}