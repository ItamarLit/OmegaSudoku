using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Interfaces
{
    public interface IinputReader
    {
        public string Input { get; set; }
        public void GetUserInput();
    }
}
