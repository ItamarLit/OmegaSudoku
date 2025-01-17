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
            OutputHandler.ShowInstructions();
            while (!endRunFlag) 
            {
                try
                {
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
