namespace RecorredorNonograma.Models;

public sealed class NonogramState
{
    private readonly List<bool[]> _rows = [];
    
    public int Width { get; }
    public int Height { get; }
    
    public int Depth => _rows.Count;
    public bool IsComplete => Depth == Height;
    
    public NonogramState(NonogramPuzzle puzzle)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        
        Width = puzzle.Width;
        Height = puzzle.Height;
    }
    
    public void AddRow(bool[] row)
    {
        ArgumentNullException.ThrowIfNull(row);
        
        if (IsComplete)
            throw new InvalidOperationException("El tablero ya está completo.");
        
        if (row.Length != Width)
            throw new ArgumentException("La longitud de la fila no coincide con el ancho.");
        
        _rows.Add((bool[])row.Clone());
    }

    public void RemoveLastRow()
    {
        if (Depth == 0)
            throw new InvalidOperationException("No hay filas para eliminar.");
        
        _rows.RemoveAt(Depth - 1);
    }
    
    public bool GetCell(int row, int column)
    {
        if (row < 0 || row >= Depth)
            throw new ArgumentOutOfRangeException(nameof(row));
        
        if (column < 0 || column >= Width)
            throw new ArgumentOutOfRangeException(nameof(column));
        
        return _rows[row][column];
    }
}