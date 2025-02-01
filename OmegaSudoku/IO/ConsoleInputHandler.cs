using OmegaSudoku.Interfaces;

namespace OmegaSudoku.IO
{
    class ConsoleInputHandler : IinputReader
    {
        /// <summary>
        /// This is the class used to handle all the input functions
        /// the class will check to see that the board is valid and then the board can be attempted to solve
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