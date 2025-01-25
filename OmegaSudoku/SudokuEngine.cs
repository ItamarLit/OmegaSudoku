using System;
using OmegaSudoku;
using OmegaSudoku.Exceptions;
using OmegaSudoku.GameLogic.Heurisitcs;
using OmegaSudoku.GameLogic;
using OmegaSudoku.IO;

namespace OmegaSudoku
{
    class SudokuEngine
    {
        public static void RunEngine()
        {
            bool endRunFlag = false;
            Console.WriteLine("Hi welcome to the amazing sudoku engine, N * N board, to exit the engine write: EXIT");
            while (!endRunFlag) 
            {
                try
                {
                    SudokuSolver.depth = 0;
                    OutputHandler.RequestBoard();
                    InputHandler.GetUserInput();
                    if (!InputHandler.CheckInput())
                    {
                        endRunFlag = true;
                    }
                    else
                    {
                        BoardCell[,] board = InputHandler.SetUpBoard();
                        OutputHandler.PrintBoard(board);
                        Mrvdict mrvArray = new Mrvdict(board.GetLength(0));
                        SudokuSolver solver = new SudokuSolver(board, mrvArray);
                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                        if (solver.Solve())
                        {
                            OutputHandler.PrintBoard(board);
                        }
                        else
                        {
                            OutputHandler.ShowImpossibleBoardMsg();

                        }
                        stopwatch.Stop();
                        OutputHandler.ShowProgramRuntime(stopwatch.ElapsedMilliseconds);
                        Console.WriteLine($"Depth: {SudokuSolver.depth}");
                    }
                }
                catch (Exception e)
                {
                    //The only excptions that can occur are mine so i can catch Exception
                    OutputHandler.OutputError(e.Message);
                }

            }
        }
    }
}
