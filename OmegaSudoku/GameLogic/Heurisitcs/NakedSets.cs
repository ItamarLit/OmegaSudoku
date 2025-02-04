using OmegaSudoku.Interfaces;
using System.Collections.Generic;
using System;


namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public class NakedSets : IHeuristic
    {
        /// <summary>
        /// This class is used to apply the naked set heuristic to the board, a naked set occurs when n cells have exactly the same n values
        /// you can then remove the cells values from the other cells in the unit as the values have to go into those cells
        /// </summary>
        /// 
        private readonly int _setSize;

        public NakedSets(int setSize)
        {
            _setSize = setSize;
        }

        /// <summary>
        /// This func is used to apply the naked sets heuristic to the board using the mrvDict to get cells with exactly n possibilities
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <param name="logicHandler"></param>
        /// <param name="mrvInstance"></param>
        /// <returns>The func returns false if the board ends up in an invalid state, else it returns true.</returns>
        public bool ApplyHeuristic(StateChange currentState, int row, int col, ICell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance)
        {
            // get the cells with n possiblites
            HashSet<ICell> setCells = mrvInstance.GetCellsWithPossibilites(_setSize);
            if (setCells == null || setCells.Count < _setSize)
            {
                // not enough cells on board with n possibilites
                return true;
            }
            // filter the n set of cells into there units
            List<List<ICell>[]> filteredUnits = FilterIntoUnits(setCells, board.GetLength(0));
            // go over rows, cols and cubes
            for (int i = 0; i < filteredUnits.Count; i++)
            {
                // go over every unit in the list of rows, cols and cubes
                foreach (var unit in filteredUnits[i])
                {
                    Dictionary<int, List<ICell>> filterDict = FilterByPossiblites(unit, board);
                    foreach (var keyVal in filterDict)
                    {
                        if (filterDict[keyVal.Key].Count == _setSize)
                        {
                            // found naked set
                            ICell cell = filterDict[keyVal.Key][0];
                            // get the unit cells for the set
                            HashSet<ICell> unitCells = GetCorrectUnitCells(filterDict[keyVal.Key], logicHandler, board.GetLength(0));
                            // remove the possibilities from the rest of the unit
                            if (!HeuristicUtils.RemoveRedundantPossibilities(unitCells, filterDict[keyVal.Key], cell.GetCellPossibilities(), 0, mrvInstance, currentState))
                            {
                                return false;

                            }
                            
                        }

                    }
                }
            }
            return true;
        }


        /// <summary>
        /// This func is used to filter the cells of n possibilities into there units
        /// </summary>
        /// <param name="setCells"></param>
        /// <param name="boardSize"></param>
        /// <returns>Returns a list of rows, cols and cubes lists that hold the cells that are in their unit</returns>
        private static List<List<ICell>[]> FilterIntoUnits(HashSet<ICell> setCells, int boardSize)
        {
            List<ICell>[] rows = new List<ICell>[boardSize];
            List<ICell>[] cols = new List<ICell>[boardSize];
            List<ICell>[] cubes = new List<ICell>[boardSize];
            // init the lists
            for (int i = 0; i < boardSize; i++)
            {
                rows[i] = new List<ICell>();
                cols[i] = new List<ICell>();
                cubes[i] = new List<ICell>();
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
            List<List<ICell>[]> units = new List<List<ICell>[]>();
            units.Add(rows);
            units.Add(cols);
            units.Add(cubes);
            return units;
        }

        /// <summary>
        /// This func filters the cells in a unit by there bitmask
        /// </summary>
        /// <param name="unitSet"></param>
        /// <param name="board"></param>
        /// <returns>Dict of bitmask and cells </returns>
        private static Dictionary<int, List<ICell>> FilterByPossiblites(List<ICell> unitSet, ICell[,] board)
        {
            Dictionary<int, List<ICell>> filterDict = new Dictionary<int, List<ICell>>();
            foreach (var cell in unitSet)
            {
                // get the bitmask
                int possibilites = cell.GetCellPossibilities();
                // add the bitmask to the dict
                if(filterDict.TryGetValue(possibilites, out var cellList))
                {
                    cellList.Add(cell);
                }
                else
                {
                    filterDict[possibilites] = new List<ICell> { cell };
                }
            }
            return filterDict;
        }

        /// <summary>
        /// This func is used to get the correct unit cells for a set of cells, if 2 cells are in the same row and cube then the func returns
        /// The cells in the row and the cells in the cube with no duplicates (using hashsets)
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="logicHandler"></param>
        /// <param name="boardSize"></param>
        /// <returns>Hashset of cells that are the correct cells that are in the unit of the setcells</returns>
        private static HashSet<ICell> GetCorrectUnitCells(IEnumerable<ICell> cells, SudokuLogicHandler logicHandler, int boardSize)
        {
            HashSet<ICell> result = new HashSet<ICell>();
            // check if they are all in the same row
            if (AllInSameRow(cells))
            {
                int row = cells.First().CellRow;
                result.UnionWith(logicHandler.GetRowCells(row));
            }
            // check if they are all in the same col
            if (AllInSameColumn(cells))
            {
                int col = cells.First().CellCol;
                result.UnionWith(logicHandler.GetColumnCells(col));
            }
            // check if they are all in the same cube
            if (AllInSameCube(cells, boardSize))
            {
                int row = cells.First().CellRow;
                int col = cells.First().CellCol;
                result.UnionWith(logicHandler.GetCubeCells(row, col));
            }
            // return the unit cells
            return result;
        }

        private static bool AllInSameRow(IEnumerable<ICell> cells)
        {
            // check if all the cells are in the same row
            int firstRow = cells.First().CellRow;
            return cells.All(c => c.CellRow == firstRow);
        }

        private static bool AllInSameColumn(IEnumerable<ICell> cells)
        {
            // check if all the cells are in the same col
            int firstCol = cells.First().CellCol;
            return cells.All(c => c.CellCol == firstCol);
        }

        private static bool AllInSameCube(IEnumerable<ICell> cells, int boardSize)
        {
            // check if all the cells are in the same cubr
            int boxSize = (int)Math.Sqrt(boardSize);
            ICell first = cells.First();
            int firstBoxRow = first.CellRow / boxSize;
            int firstBoxCol = first.CellCol / boxSize;

            return cells.All(c =>
                (c.CellRow / boxSize == firstBoxRow) &&
                (c.CellCol / boxSize == firstBoxCol)
            );
        }

    }
}