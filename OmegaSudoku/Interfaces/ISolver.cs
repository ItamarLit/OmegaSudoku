using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Interfaces
{
    public interface ISolver
    {   
        // interface for solver, used so any solver can solve the baord (implementation doesn't matter)
        public bool Solve();
    }
}
