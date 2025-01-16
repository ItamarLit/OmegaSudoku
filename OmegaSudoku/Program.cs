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
            SudokuLogicHandler sc = new SudokuLogicHandler(board, mrvArray);
            sc.CheckInitalBoard();
            sc.SetInitailBoardPossibilites();
            OutputHandler.PrintBoardPossibilites(board);
            mrvArray.PrintArray();
        }
    }
}