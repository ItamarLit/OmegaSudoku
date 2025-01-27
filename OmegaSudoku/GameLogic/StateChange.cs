using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic
{
    public class StateChange
    {
        /// <summary>
        /// This class is used to hold data of changes during backtracking, 
        /// it has a hash set of cellValue changes and a hash set of cellPossibility changes
        /// </summary>
        public List<(int row, int col, int oldValue)> CellValueChanges;
        public List<(int row, int col, int removedValue)> CellPossibilityChanges;

        public StateChange()
        {
            CellValueChanges = new List<(int row, int col, int oldValue)>();
            CellPossibilityChanges = new List<(int row, int col, int removedValue)>();
        }
    }
}
