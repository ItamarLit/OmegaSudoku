﻿using System;
using System.Collections.Generic;


namespace OmegaSudoku.GameLogic
{
    public class StateChange
    {
        /// <summary>
        /// This class is used to hold data of changes during backtracking, 
        /// it has a list of cellValue changes and a list of cellPossibility changes
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
