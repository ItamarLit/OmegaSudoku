using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmegaSudoku;
using OmegaSudoku.Exceptions;
using OmegaSudoku.GameLogic;
using OmegaSudoku.IO;

namespace OmegaSudoku
{
    class SudokuEngine
    {
        public static void RunEngine()
        {
            bool endRunFlag = false;
            Console.WriteLine("Hi welcome to the amazing sudoku engine, enter any 9x9 board, to exit the engine write: EXIT");
            while (!endRunFlag) 
            {
                try
                {   
                    OutputHandler.RequestBoard();
                    InputHandler.GetUserInput();
                    DateTime start = DateTime.Now;
                    if (!InputHandler.CheckInput())
                    {
                        endRunFlag = true;
                    }
                    else
                    {
                        BoardCell[,] board = InputHandler.SetUpBoard();
                        OutputHandler.PrintBoard(board);
                        MrvArray mrvArray = new MrvArray(board.GetLength(0));
                        SudokuSolver solver = new SudokuSolver(board, mrvArray);
                        if (solver.Solve())
                        {
                            OutputHandler.PrintBoard(board);
                        }
                        else
                        {
                            OutputHandler.ShowImpossibleBoardMsg();
                        }
                        DateTime end = DateTime.Now;
                        TimeSpan elapsed = end - start;
                        OutputHandler.ShowProgramRuntime(elapsed);
                    }
                }
                // The only excptions that can occur are mine so i can catch Exception
                catch (Exception e) 
                {
                    OutputHandler.OutputError(e.Message);
                }
               
            }
        }
    }
}
