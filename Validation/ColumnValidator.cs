using RecorredorNonograma.Models;

namespace RecorredorNonograma.Validation;

public static class ColumnValidator
{
    private static bool IsCompatible(NonogramState state, int columnIndex, List<bool[]> validPatterns)
    {
        foreach (var pattern in validPatterns)
        {
            var matches = true;
            
            for (var rowIndex = 0; rowIndex < state.Depth; rowIndex++)
            {
                var cellState = state.GetCell(rowIndex, columnIndex);
                if (cellState == pattern[rowIndex]) continue;
                matches = false;
                break;
            }
            if (matches) return true;
        }
        return false;
    }
    
    public static bool AreAllCompatible(NonogramState state, List<bool[]>[] columnPatterns)
    {
        for (var columnIndex = 0; columnIndex < state.Width; columnIndex++)
        {
            if (!IsCompatible(state, columnIndex, columnPatterns[columnIndex]))
                return false;
        }
        return true;
    }
}