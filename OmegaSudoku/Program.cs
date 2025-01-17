using OmegaSudoku.GameLogic;
using OmegaSudoku.IO;
using System;
namespace OmegaSudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            
            OutputHandler.RequestBoard();
            InputHandler.GetUserInput();
            InputHandler.CheckInput();
            BoardCell[,] board = InputHandler.SetUpBoard();
            OutputHandler.PrintBoard(board);
            MrvArray mrvArray = new MrvArray(board.GetLength(0));
            SudokuSolver ss = new SudokuSolver(board, mrvArray);
            OutputHandler.PrintBoard(board);
            //OutputHandler.PrintBoard(board);
            
            //mrvArray.PrintArray();
        }
    }
}