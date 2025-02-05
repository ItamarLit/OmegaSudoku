namespace OmegaSudoku.Exceptions
{
    public class BoardSizeException : Exception
    {
        // This exception is thrown when the input validator finds that the entered input is of invalid size
        public BoardSizeException(int invalidBoardSize)
            : base($"Invalid board size entered: {invalidBoardSize}") 
        {
        
        }
    }
}
