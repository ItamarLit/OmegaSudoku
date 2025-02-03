namespace OmegaSudoku.Exceptions
{
    public class EmptyInputException : Exception
    {
        public EmptyInputException()
            : base("Invalid empty input")
        { }
    }
}
