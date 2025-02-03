using OmegaSudoku.Interfaces;


namespace OmegaSudoku.IO
{
    public class FileInputHandler : IinputReader
    {
        public string Input { get; set; }

        private readonly string _filepath;

        public FileInputHandler(string filePathInput)
        {
            if (string.IsNullOrWhiteSpace(filePathInput))
            {
                throw new ArgumentException("File path cannot be null or empty.");
            }
            // clean the file path from " "
            filePathInput = filePathInput.TrimStart('"');
            filePathInput = filePathInput.TrimEnd('"');
            _filepath = filePathInput;
        }

        public void GetUserInput()
        {
            if (!File.Exists(_filepath))
            {
                throw new FileNotFoundException($"The file at: {_filepath} was not found.");
            }
            Input = File.ReadAllText(_filepath);
        }

        public string GetPath()
        {
            return _filepath;
        }
    }
}
