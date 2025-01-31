using OmegaSudoku.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSetsUtil : IHeuristic
    {
        private int _setSize;

        public NakedSetsUtil(int setSize)
        {
            _setSize = setSize;
        }

        public bool ApplyHeuristic(StateChange currentState, int row, int col, Icell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the cells with n possiblites
            HashSet<Icell> setCells = mrvInstance.GetCellsWithPossibilites(_setSize);
            if (setCells == null || setCells.Count < _setSize)
            {
                // no cells on board with n possibilites
                return true;
            }
            // filter the n set of cells into there units
            List<List<Icell>[]> filteredUnits = FilterIntoUnits(setCells, board.GetLength(0));
            // go over rows, cols and cubes
            for (int i = 0; i < filteredUnits.Count; i++)
            {
                // go over every unit in the list of rows, cols and cubes
                foreach (var unit in filteredUnits[i])
                {
                    Dictionary<int, List<Icell>> filterDict = FilterByPossiblites(unit, board);
                    foreach (var keyVal in filterDict)
                    {
                        if (filterDict[keyVal.Key].Count == _setSize)
                        {
                            // found naked set
                            Icell cell = filterDict[keyVal.Key][0];
                            HashSet<Icell> unitCells = GetCorrectUnitCells(filterDict[keyVal.Key], logicHandler, board.GetLength(0));
                            if (!HeuristicUtils.RemoveRedundantPossibilities(unitCells, filterDict[keyVal.Key], cell.GetCellMask(), 0, mrvInstance, currentState))
                            {
                                return false;

                            }
                            
                        }

                    }
                }
            }
            return true;
        }


        private static List<List<Icell>[]> FilterIntoUnits(HashSet<Icell> setCells, int boardSize)
        {
            // this func will filter each cell into its correct unit
            List<Icell>[] rows = new List<Icell>[boardSize];
            List<Icell>[] cols = new List<Icell>[boardSize];
            List<Icell>[] cubes = new List<Icell>[boardSize];
            // init the lists
            for (int i = 0; i < boardSize; i++)
            {
                rows[i] = new List<Icell>();
                cols[i] = new List<Icell>();
                cubes[i] = new List<Icell>();
            }
            int cubeSize = (int)Math.Sqrt((double)boardSize);
            foreach (var cell in setCells)
            {
                // filter the cells into there units
                rows[cell.CellRow].Add(cell);
                cols[cell.CellCol].Add(cell);
                int cubeIndex = (cell.CellRow / cubeSize) * cubeSize + (cell.CellCol / cubeSize);
                cubes[cubeIndex].Add(cell);
            }
            // list to hold all unit lists
            List<List<Icell>[]> units = new List<List<Icell>[]>();
            units.Add(rows);
            units.Add(cols);
            units.Add(cubes);
            return units;
        }

        private static Dictionary<int, List<Icell>> FilterByPossiblites(List<Icell> unitSet, Icell[,] board)
        {
            // this func filters the cells in a unit with n possiblites into a dict by there different possiblites
            Dictionary<int, List<Icell>> filterDict = new Dictionary<int, List<Icell>>();
            foreach (var cell in unitSet)
            {
                // get the cell from the board
                int possibilites = cell.GetCellMask();
                bool foundKey = false;
                if(filterDict.TryGetValue(possibilites, out var cellList))
                {
                    cellList.Add(cell);
                }
                else
                {
                    filterDict[possibilites] = new List<Icell> { cell };
                }
            }
            return filterDict;
        }

        private static HashSet<Icell> GetCorrectUnitCells(IEnumerable<Icell> cells, SudokuLogicHandler logicHandler, int boardSize)
        {
            // this func will get the unit cells for the naked cells
            HashSet<Icell> result = new HashSet<Icell>();

            if (AllInSameRow(cells))
            {
                int row = cells.First().CellRow;
                result.UnionWith(logicHandler.GetRowCells(row));
            }

            if (AllInSameColumn(cells))
            {
                int col = cells.First().CellCol;
                result.UnionWith(logicHandler.GetColumnCells(col));
            }

            if (AllInSameCube(cells, boardSize))
            {
                int row = cells.First().CellRow;
                int col = cells.First().CellCol;
                result.UnionWith(logicHandler.GetCubeCells(row, col));
            }

            return result;
        }

        private static bool AllInSameRow(IEnumerable<Icell> cells)
        {
            // check if all the cells are in the same row
            int firstRow = cells.First().CellRow;
            return cells.All(c => c.CellRow == firstRow);
        }

        private static bool AllInSameColumn(IEnumerable<Icell> cells)
        {
            // check if all the cells are in the same col
            int firstCol = cells.First().CellCol;
            return cells.All(c => c.CellCol == firstCol);
        }

        private static bool AllInSameCube(IEnumerable<Icell> cells, int boardSize)
        {
            // check if all the cells are in the same cubr
            int boxSize = (int)Math.Sqrt(boardSize);
            Icell first = cells.First();
            int firstBoxRow = first.CellRow / boxSize;
            int firstBoxCol = first.CellCol / boxSize;

            return cells.All(c =>
                (c.CellRow / boxSize == firstBoxRow) &&
                (c.CellCol / boxSize == firstBoxCol)
            );
        }

    }
}