using System;
using OmegaSudoku.MinHeap;
using OmegaSudoku.IO;
using System.Collections.Generic;

namespace OmegaSudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                OutputHandler.RequestBoard();
                InputHandler.GetInput();
                BoardCell[,] board = InputHandler.CheckInput();
                OutputHandler.PrintBoard(board);
            }
           
        }
    }
}
