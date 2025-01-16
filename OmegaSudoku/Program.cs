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
        }
    }
}