namespace OmegaSudoku.Interfaces
{
    public interface ICell
    {
        // This interface is for a cell on the board

        // properties that every cell has
        public int CellCol { get; }
        public int CellRow { get; }
        public int CellValue { get; set; }

        // func that removes a possibility from a cell
        public void DecreasePossibility(int possibiltiyValue);

        // func that adds a possibility to a cell
        public void IncreasePossibility(int possibilityValue);

        // func that returns num possiblites in a cell
        public int NumberOfPossibilites();

        // func that checks if a cell is empty
        public bool IsCellEmpty();
        // func thar returns if a cell has a specific value
        public bool HasValue(int value);
        // func that returns the possiblites of a cell
        public List<int> GetPossibilites();

        // functions to get cell info
        public void SetCellPossibilities(int value);

        public int GetCellPossibilities();

        // get the final value of a cell ( the value it has to be out of the possibilities)
        public int GetFinalCellValue();
    }
}
