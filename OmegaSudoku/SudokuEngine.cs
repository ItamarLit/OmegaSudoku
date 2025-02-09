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
            // create an event handler to handle ctrl c
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("\nThe sudoku was forcefully closed, bye!");
                // close with correct exit code
                Environment.Exit(0);
            };
            // main engine func, used to get the user input and choices
            bool endRunFlag = false;
            Console.WriteLine("*** Welcome to the Amazing Sudoku Solver ***\n");
            OutputHandler.ShowMenu();
            ConsoleInputHandler consoleInputHandler = new ConsoleInputHandler();
            while (!endRunFlag)
            {
                Console.Write("\n\nEnter your choice: ");
                consoleInputHandler.GetUserInput();
                if(consoleInputHandler.Input == null)
                {
                    Console.WriteLine("Invalid choice entered.");
                }
                else
                {
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
                            Console.WriteLine("Invalid choice entered.");
                            break;
                    }
                }
            }
        }

        private static void SolveBoardFromConsole(ConsoleInputHandler consoleInputHandler)
        {
            // func to solve boards from console
            try
            {
                Console.WriteLine("Enter the Sudoku board:");
                consoleInputHandler.GetUserInput();
                if(consoleInputHandler.Input == null)
                {
                    throw new ArgumentException("Board input cannot be null, please try again.");
                }
                string input = consoleInputHandler.Input.Trim();
                SolveBoard(input, false, consoleInputHandler);
            }
            catch (Exception e)
            {
                OutputHandler.OutputError(e.Message);
            }
        }

        private static void SolveBoardFromFile(ConsoleInputHandler consoleInputHandler)
        {
            // func to solve boards from file
            try
            {
                Console.Write("Enter the file path of the Sudoku board: ");
                consoleInputHandler.GetUserInput();
                // get the file path
                string path = consoleInputHandler.Input;
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

        /// <summary>
        /// This is the main func in the engine, it is used to attempt to solve the board and 
        /// also calls funcs that show the output of the attempt
        /// </summary>
        /// <param name="input"></param>
        /// <param name="is_file"></param>
        /// <param name="inputHandler"></param>
        private static void SolveBoard(string input, bool is_file, IInputReader inputHandler)
        {
            // check the input
            InputValidator.CheckInput(input);
            // setup the board
            ICell[,] board = InputValidator.SetUpBoard(input);
            Console.WriteLine("Attempting to solve your board:");
            // set up solver
            ISolver solver = new SudokuSolver(board);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            bool solved = solver.Solve();
            stopwatch.Stop();
            OutputHandler.ShowOutput(board, solved);
            // if the input is from a file write the output
            if (is_file)
            {
                string filePath = ((FileInputHandler)inputHandler).GetPath();
                OutputHandler.WriteIntoFile(filePath, board, solved);
            }
            Console.WriteLine($"The program ran for: {stopwatch.ElapsedMilliseconds} milliseconds");

        }
    }
}
