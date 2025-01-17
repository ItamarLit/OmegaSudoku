using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Exceptions
{
    class BoardInfoException : Exception
    {
        public BoardInfoException() 
            : base("The board you entered doesn't contain only numbers.") 
        { } 

    }
}
