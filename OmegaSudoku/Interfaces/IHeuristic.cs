using OmegaSudoku.GameLogic;


namespace OmegaSudoku.Interfaces
{
    public interface IHeuristic
    {
        // This interface is used for all set heuristics (naked / hidden sets)
        public bool ApplyHeuristic(StateChange currentState, int row, int col, ICell[,] board, SudokuLogicHandler logicHandler, Mrvdict mrvInstance);
    }
}
