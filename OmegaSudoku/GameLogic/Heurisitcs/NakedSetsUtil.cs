using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSetsUtil
    {
        public static bool ApplyNakedSets(StateChange currentState, BoardCell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance, int setSize)
        {
            HashSet<(int, int)> setCells = mrvInstance.GetCellsWithPossibilites(setSize);
            if (setCells == null) 
            {
                // no cells on board with n possibilites
                return true;
            }
            // filter the n set of cells into there units
            List<List<(int, int)>[]> filteredUnits = FilterIntoUnits(setCells, board.GetLength(0));
            // go over rows, cols and cubes
            for (int i = 0; i < filteredUnits.Count; i++)
            {
                // go over every unit in the list of rows, cols and cubes
                foreach(var unit in filteredUnits[i])
                {
                    Dictionary<List<int>, List<(int, int)>> filterDict = FilterByPossiblites(unit, board);
                    foreach(var keyVal in filterDict)
                    {
                        if (filterDict[keyVal.Key].Count == setSize)
                        {
                            // found naked set
                            (int, int) cell = filterDict[keyVal.Key][0];
                            HashSet<(int, int)> unitCells = logicHandler.GetUnitCellsPos(cell.Item1, cell.Item2);
                            HashSet<BoardCell> cellUnitCells = logicHandler.GetCellsByIndexes(unitCells);
                            if (!RemoveRedundantPossibilites(cellUnitCells, GetBoardCells(filterDict[keyVal.Key], board), keyVal.Key, mrvInstance, currentState)) 
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        private static List<List<(int, int)>[]> FilterIntoUnits(HashSet<(int, int)> setCells, int boardSize)
        {
            // this func will filter each cell into its correct unit
            List<(int, int)>[] rows = new List<(int, int)>[boardSize];
            List<(int, int)>[] cols = new List<(int, int)>[boardSize];
            List<(int, int)>[] cubes = new List<(int, int)>[boardSize];
            // init the lists
            for (int i = 0; i < boardSize; i++)
            {
                rows[i] = new List<(int, int)>();
                cols[i] = new List<(int, int)>();
                cubes[i] = new List<(int, int)>();
            }
            int cubeSize = (int)Math.Sqrt((double)boardSize);
            foreach ((int row, int col) cell in setCells) 
            {
                // filter the cells into there units
                rows[cell.row].Add(cell);
                cols[cell.col].Add(cell);
                int cubeIndex = (cell.row / cubeSize) * cubeSize + (cell.col / cubeSize);
                cubes[cubeIndex].Add(cell);
            }
            // list to hold all unit lists
            List<List<(int, int)>[]> units = new List<List<(int, int)>[]>();
            units.Add(rows);
            units.Add(cols);
            units.Add(cubes);
            return units;
        }

        private static Dictionary<List<int>, List<(int, int)>> FilterByPossiblites(List<(int, int)> unitSet, BoardCell[,] board)
        {
            // this func filters the cells in a unit with n possiblites into a dict by there different possiblites
            Dictionary<List<int>, List<(int, int)>> filterDict = new Dictionary<List<int>, List<(int, int)>>();
            foreach (var unit in unitSet) 
            {
                // get the cell from the board
                BoardCell cell = board[unit.Item1, unit.Item2];
                List<int> possibilites = cell.GetPossibilites().OrderBy(p => p).ToList();
                bool foundKey = false;
                foreach (var key in filterDict.Keys)
                {
                    if (key.SequenceEqual(possibilites))
                    {
                        filterDict[key].Add(unit);
                        foundKey = true;
                    }
                    if (!foundKey)
                    {
                        filterDict[possibilites] = new List<(int, int)> { unit };
                    }
                }
            }
            return filterDict;
        }

        public static bool RemoveRedundantPossibilites(IEnumerable<BoardCell> unitCells, List<BoardCell> setCells, List<int> possibilites, Mrvdict mrvInstance, StateChange currentState)
        {
            
            foreach (BoardCell cell in unitCells)
            {
                if (cell.CellValue == 0 && !setCells.Contains(cell))
                {
                    mrvInstance.RemoveCell(cell);
                    // remove the possibilites from the cells that arent in the pair
                    foreach (int possibility in possibilites)
                    {
                        if (cell.HasValue(possibility))
                        {
                            cell.DecreasePossibility(possibility);
                            currentState.CellPossibilityChanges.Add((cell.CellRow, cell.CellCol, possibility));
                        }
                    }
                    // if a cell has no possiblites then the board is invalid
                    if (cell.NumberOfPossibilites() == 0)
                    {
                        return false;
                    }
                    mrvInstance.InsertCell(cell);
                }

            }
            return true;
        }

        private static List<BoardCell> GetBoardCells(List<(int, int)> cells, BoardCell[,] board)
        {
            List<BoardCell> boardCellcells = new List<BoardCell>();
            foreach(var cell in cells)
            {
                boardCellcells.Add(board[cell.Item1, cell.Item2]);
            }
            return boardCellcells;
        }
    }
}
