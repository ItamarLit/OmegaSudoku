
namespace OmegaSudoku.GameLogic.Heurisitcs
{
    public enum HeuristicResult
    {
        /// <summary>
        /// Enum for returning results from the applyHeuristics func in the solver
        /// </summary>
        NoChange,
        ProgressMade,
        InvalidState,
    }
}
