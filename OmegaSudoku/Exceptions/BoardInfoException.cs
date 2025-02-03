namespace OmegaSudoku.Exceptions
{
    class BoardInfoException : Exception
    {
        public BoardInfoException() 
            : base("The board you entered doesn't contain only numbers.") 
        { } 

    }
}
