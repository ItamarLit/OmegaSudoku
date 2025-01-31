using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Interfaces
{
    public interface IinputReader
    {
        // Interface for input readers, (file / console)
        public string Input { get; set; }
        public void GetUserInput();
    }
}
