using RecorredorNonograma.Models;
using RecorredorNonograma.Validation;

namespace RecorredorNonograma.Search;

public sealed class NonogramSolver
{
    private readonly NonogramPuzzle _puzzle;
    private readonly List<bool[]>[] _rowPatterns;
    private readonly List<bool[]>[] _columnPatterns;
    
    public long NodesExpanded { get; private set; }
    public TimeSpan ElapsedTime { get; private set; }
    public long AllocatedBytes { get; private set; }

    public NonogramSolver(NonogramPuzzle puzzle)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        
        _puzzle = puzzle;
        
        _rowPatterns = new List<bool[]>[puzzle.Height];
        _columnPatterns = new List<bool[]>[puzzle.Width];
        
        var rowClues = puzzle.RowClues;
        var columnClues = puzzle.ColumnClues;
        
        for (var i = 0; i < puzzle.Height; i++)
            _rowPatterns[i] = RowPatternGenerator.Generate(puzzle.Width, rowClues[i]);
        
        for (var i = 0; i < puzzle.Width; i++)
            _columnPatterns[i] = RowPatternGenerator.Generate(puzzle.Height, columnClues[i]);
    }
    
    public NonogramState? Solve()
    {
        var state = new NonogramState(_puzzle);
        
        NodesExpanded = 0;
        var startBytes = GC.GetAllocatedBytesForCurrentThread();
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var success = Search(state);
        stopwatch.Stop();
        
        ElapsedTime = stopwatch.Elapsed;
        AllocatedBytes = GC.GetAllocatedBytesForCurrentThread() - startBytes;
        
        return success ? state : null;
    }
    
    private bool Search(NonogramState state)
    {
        if (state.IsComplete) return true;
        NodesExpanded++;
        
        var rowIndex = state.Depth;
        foreach (var pattern in _rowPatterns[rowIndex])
        {
            state.AddRow(pattern);
            if (ColumnValidator.AreAllCompatible(state, _columnPatterns) && Search(state))
            {
                return true;
            }
            state.RemoveLastRow();
        }
        return false;
    }
}