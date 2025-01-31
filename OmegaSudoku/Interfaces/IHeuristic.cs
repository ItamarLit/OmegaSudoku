using OmegaSudoku.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Interfaces
{
    public interface IHeuristic
    {
        // This interface is used for all set heuristics (naked / hidden sets)
        public bool ApplyHeuristic(StateChange currentState, int row, int col, Icell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance);
    }
}
