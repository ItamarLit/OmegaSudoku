namespace OmegaSudoku.Exceptions
{
    class CellInfoExeption : Exception
    {
        // This exception is thrown when a cell in the input has an invalid value (not in the correct value range)
        public CellInfoExeption(char cellValue) 
            : base($"Invalid cell value entered: {cellValue}.") 
        { }
    }
}
