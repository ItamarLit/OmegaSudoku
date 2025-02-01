
namespace OmegaSudoku.Exceptions
{
    class BoardSizeException : Exception
    {
        public BoardSizeException(int invalidBoardSize)
            : base($"Invalid board size entered: {invalidBoardSize}") 
        {
        
        }
    }
}
