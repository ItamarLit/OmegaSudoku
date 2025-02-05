using OmegaSudoku.Interfaces;

namespace OmegaSudoku.IO
{
    class ConsoleInputHandler : IInputReader
    {
        /// <summary>
        /// This is the class used to handle all console input 
        /// </summary>

        public string Input { get; set; }

        // Func to get the user input
        public void GetUserInput()
        {
            // get the users input
            Input = Console.ReadLine();
        }

    }
}