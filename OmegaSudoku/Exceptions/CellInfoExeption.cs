using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Exceptions
{
    class CellInfoExeption : Exception
    {
        public CellInfoExeption(int cellValue) 
            : base($"Invalid cell value entered: {cellValue}.") 
        { }
    }
}
