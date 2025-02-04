namespace OmegaSudoku.Interfaces
{
    public interface IInputReader
    {
        // Interface for input readers, (file / console)
        public string Input { get; set; }
        public void GetUserInput();
    }
}
