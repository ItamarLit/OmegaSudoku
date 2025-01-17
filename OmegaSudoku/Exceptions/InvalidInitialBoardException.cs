using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Exceptions
{
    class InvalidInitialBoardException : Exception
    {
        public InvalidInitialBoardException() 
            : base("The inital board you entered is invalid, there are duplicate values in a unit on the board.") 
        { }
    }
}
