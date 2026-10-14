using RecorredorNonograma.Models;

namespace RecorredorNonograma.Validation;

public static class NonogramSolutionValidator
{
    private static int[] ExtractClues(bool[] cells)
    {
        ArgumentNullException.ThrowIfNull(cells);
        
        List<int> clues = [];
        var count = 0;
        
        foreach (var cell in cells)
        {
            if (cell) count++;
            else if (count > 0)
            {
                clues.Add(count);
                count = 0;
            }
        }
        if (count > 0) clues.Add(count);
        return [.. clues];
    }

    public static bool IsValid(NonogramPuzzle puzzle, NonogramState state)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        ArgumentNullException.ThrowIfNull(state);
        
        if (!state.IsComplete || state.Width != puzzle.Width || state.Height != puzzle.Height)
            return false;
        
        var rowClues = puzzle.RowClues;
        var columnClues = puzzle.ColumnClues;
        
        for (var row = 0; row < puzzle.Height; row++)
        {
            var cells = new bool[puzzle.Width];
            
            for (var column = 0; column < puzzle.Width; column++)
                cells[column] = state.GetCell(row, column);
            
            if (!ExtractClues(cells).SequenceEqual(rowClues[row]))
                return false;
        }

        for (var column = 0; column < puzzle.Width; column++)
        {
            var cells = new bool[puzzle.Height];
            
            for (var row = 0; row < puzzle.Height; row++)
                cells[row] = state.GetCell(row, column);
            
            if (!ExtractClues(cells).SequenceEqual(columnClues[column]))
                return false;
        }
        return true;
    }
}