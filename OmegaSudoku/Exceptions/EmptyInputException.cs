namespace OmegaSudoku.Exceptions
{
    public class EmptyInputException : Exception
    {
        // This exception is thrown when the user inputs nothing
        public EmptyInputException()
            : base("Invalid empty input, please try again.")
        { }
    }
}
