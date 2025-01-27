using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Interfaces
{
    public interface Icell
    {
        // func that removes possiblites from a cell
        public void DecreasePossibility(int possibiltiyValue);

        // func that adds possiblites from a cell
        public void IncreasePossibility(int possibilityValue);

        // func that returns num possiblites in a cell
        public int NumberOfPossibilites();

        // func that checks if a cell is empty
        public bool IsCellEmpty();
        // func thar returns if a cell has a specific value
        public bool HasValue(int value);
        // func that returns the possiblites of a cell
        public HashSet<int> GetPossibilites();

        // functions to get cell info

        public int GetCellRow();

        public int GetCellCol();

        public int GetCellValue();

        public void SetCellValue(int value);
    }
}
