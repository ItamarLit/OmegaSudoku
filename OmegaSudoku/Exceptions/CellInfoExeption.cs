
namespace OmegaSudoku.Exceptions
{
    class CellInfoExeption : Exception
    {
        public CellInfoExeption(int cellValue) 
            : base($"Invalid cell value entered: {cellValue}.") 
        { }
    }
}
