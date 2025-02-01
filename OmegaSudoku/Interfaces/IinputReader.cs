

namespace OmegaSudoku.Interfaces
{
    public interface IinputReader
    {
        // Interface for input readers, (file / console)
        public string Input { get; set; }
        public void GetUserInput();
    }
}
