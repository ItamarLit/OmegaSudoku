using OmegaSudoku.GameLogic;
using OmegaSudoku.Interfaces;
using OmegaSudoku.IO;

namespace SudokuTests
{
    public class TestUtils
    {
        public static string TestBoard(string input)
        {
            //Arrange
            ICell[,] board = InputValidator.SetUpBoard(input);
            ISolver solver = new SudokuSolver(board);
            //Act
            solver.Solve();
            return OutputHandler.GetBoardStr(board);
        }

    }
}
