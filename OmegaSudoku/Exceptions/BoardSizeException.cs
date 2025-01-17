using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Exceptions
{
    class BoardSizeException : Exception
    {
        public BoardSizeException(int invalidBoardSize)
            : base($"Invalid board size entered: {invalidBoardSize}.") 
        {
        
        }
    }
}
