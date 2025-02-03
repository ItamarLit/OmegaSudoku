namespace OmegaSudoku.Exceptions
{
    class InvalidInitialBoardException : Exception
    {
        // This exception is thrown when a given inital board is invalid (has duplicates)
        public InvalidInitialBoardException() 
            : base("The inital board you entered is invalid, there are duplicate values in a unit on the board.") 
        { }
    }
}
