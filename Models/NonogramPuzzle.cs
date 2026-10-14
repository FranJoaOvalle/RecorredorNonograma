namespace RecorredorNonograma.Models;

public sealed class NonogramPuzzle
{
    public int Width { get; }
    public int Height { get; }
    
    public int[][] RowClues => CopyClues(field);
    public int[][] ColumnClues => CopyClues(field);
    
    public NonogramPuzzle(int width, int height, int[][] rowClues, int[][] columnClues)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Las dimensiones del nonograma deben ser mayores que cero.");
        
        ArgumentNullException.ThrowIfNull(rowClues);
        ArgumentNullException.ThrowIfNull(columnClues);
        
        if (rowClues.Length != height)
            throw new ArgumentException("La cantidad de pistas de filas no coincide con la altura.");
        
        if (columnClues.Length != width)
            throw new ArgumentException("La cantidad de pistas de columnas no coincide con el ancho.");
        
        ValidateClues(rowClues, width);
        ValidateClues(columnClues, height);
        
        Width = width;
        Height = height;
        
        RowClues = CopyClues(rowClues);
        ColumnClues = CopyClues(columnClues);
    }

    private static void ValidateClues(int[][] clues, int length)
    {
        foreach (var clue in clues)
        {
            if (clue is null)
                throw new ArgumentException("Las pistas no pueden ser nulas.");
            
            if (clue.Any(value => value <= 0))
                throw new ArgumentException("Las longitudes de los bloques deben ser positivas.");
            
            var required = clue.Sum(value => (long)value) + Math.Max(0, clue.Length - 1);
            
            if (required > length)
                throw new ArgumentException("Las pistas exceden las dimensiones del nonograma.");
        }
    }
    
    private static int[][] CopyClues(int[][] clues) => [.. clues.Select(clue => (int[])clue.Clone())];
}