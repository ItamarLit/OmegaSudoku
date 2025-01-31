using System;
using OmegaSudoku;
using OmegaSudoku.Exceptions;
using OmegaSudoku.GameLogic.Heurisitcs;
using OmegaSudoku.GameLogic;
using OmegaSudoku.IO;
using OmegaSudoku.Interfaces;

namespace OmegaSudoku
{
    class SudokuEngine
    {
        /// <summary>
        /// This is the engine class, it runs the solver
        /// </summary>
        /// 
        public static void RunEngine()
        {
            // main engine func, used to get the user input and choices
            bool endRunFlag = false;
            Console.WriteLine("*** Welcome to the Amazing Sudoku Solver ***\n");
            OutputHandler.ShowMenu();
            ConsoleInputHandler consoleInputHandler = new ConsoleInputHandler();
            while (!endRunFlag)
            {
                Console.Write("\n\nEnter your choice: ");
                consoleInputHandler.GetUserInput();
                string choice = consoleInputHandler.Input.Trim().ToLower();
                switch (choice)
                {
                    case "solve_c":
                        SolveBoardFromConsole(consoleInputHandler);
                        break;
                    case "solve_f":
                        SolveBoardFromFile(consoleInputHandler);
                        break;
                    case "menu":
                        OutputHandler.ShowMenu();
                        break;
                    case "rules":
                        OutputHandler.ShowRules();
                        break;
                    case "exit":
                        Console.WriteLine("Exiting the solver!");
                        endRunFlag = true;
                        break;
                    default:
                        Console.WriteLine($"Invalid choice: {choice} entered.");
                        break;
                }
            }
        }

        private static void SolveBoardFromConsole(ConsoleInputHandler consoleInputHandler)
        {
            // func to solve boards from console
            //try
            //{
                Console.WriteLine("Enter the Sudoku board:");
                consoleInputHandler.GetUserInput();
                string input = consoleInputHandler.Input.Trim();
                SolveBoard(input, false, consoleInputHandler);
            //}
            //catch (Exception e)
            //{
            //    OutputHandler.OutputError(e.Message);
            //}
        }

        private static void SolveBoardFromFile(ConsoleInputHandler consoleInputHandler)
        {
            // func to solve boards from file
            try
            {
                Console.Write("Enter the file path of the Sudoku board: ");
                consoleInputHandler.GetUserInput();
                // get the file path
                string path = consoleInputHandler.Input.Trim();
                FileInputHandler fileInputHandler = new FileInputHandler(path);
                fileInputHandler.GetUserInput();
                string input = fileInputHandler.Input.Trim();
                SolveBoard(input, true, fileInputHandler);
            }
            catch (Exception e)
            {
                OutputHandler.OutputError(e.Message);
            }
        }

        private static void SolveBoard(string input, bool is_file, IinputReader inputHandler)
        {
            // check the input
            InputValidator.CheckInput(input);
            Icell[,] board = InputValidator.SetUpBoard(input);
            OutputHandler.PrintBoard(board);
            // set up the data structs
            Mrvdict mrvDict = new Mrvdict(board.GetLength(0));
            // set up solver
            SudokuSolver solver = new SudokuSolver(board, mrvDict);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            bool invalidBoard = false;
            if (solver.Solve())
            {
                OutputHandler.PrintBoard(board);
            }
            else
            {
                OutputHandler.ShowImpossibleBoardMsg();
                invalidBoard = true;

            }
            if (is_file)
            {
                string filePath = ((FileInputHandler)inputHandler).GetPath();
                OutputHandler.WriteIntoFile(filePath, OutputHandler.GetBoardStr(board));
                if (invalidBoard)
                {
                    OutputHandler.WriteIntoFile(filePath, "The board is unsolvable");
                }
            }
            stopwatch.Stop();
            OutputHandler.ShowProgramRuntime(stopwatch.ElapsedMilliseconds);
        }
    }
}
