using OmegaSudoku.Interfaces;


namespace OmegaSudoku.IO
{
    public class FileInputHandler : IInputReader
    {
        /// <summary>
        /// This class is used to handle file input from the user
        /// </summary>
        public string Input { get; set; }

        private readonly string _filepath;

        public FileInputHandler(string filePathInput)
        {
            if (string.IsNullOrWhiteSpace(filePathInput))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }
            filePathInput = filePathInput.Trim();
            // clean the file path from " "
            filePathInput = filePathInput.Trim('"');
            _filepath = filePathInput;
        }

        public void GetUserInput()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException($"The file at: {_filepath} was not found.");
            }
            // read all data
            Input = File.ReadAllText(_filepath);
        }

        public string GetPath()
        {
            return _filepath;
        }
    }
}
