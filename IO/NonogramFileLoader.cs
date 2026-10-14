using RecorredorNonograma.Models;

namespace RecorredorNonograma.IO;

public static class NonogramFileLoader
{
    public static NonogramPuzzle Load(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("No se encontró el archivo del nonograma.", path);
        
        var lines = File.ReadAllLines(path);
        
        if (lines.Length == 0)
            throw new FormatException("El archivo está vacío.");
        
        var dimensionParts = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        if (dimensionParts.Length != 2)
            throw new FormatException("La primera línea debe contener exactamente el ancho y el alto.");
        
        var width = int.Parse(dimensionParts[0]);
        var height = int.Parse(dimensionParts[1]);
        
        if (width <= 0 || height <= 0)
            throw new FormatException("Las dimensiones deben ser positivas.");
        
        if (lines.Length != height + width + 3)
            throw new FormatException("La cantidad de líneas del archivo es incorrecta.");
        
        if (lines[1].Trim() != "ROWS" || lines[height + 2].Trim() != "COLUMNS")
            throw new FormatException("Los encabezados del archivo son inválidos.");
        
        var rowClues = new int[height][];
        for (var i = 0; i < height; i++)
        {
            rowClues[i] = ParseClues(lines[i + 2]);
        }
        
        var columnClues = new int[width][];
        for (var i = 0; i < width; i++)
        {
            columnClues[i] = ParseClues(lines[i + height + 3]);
        }
        
        return new NonogramPuzzle(width, height, rowClues, columnClues);
    }
    
    private static int[] ParseClues(string line)
    {
        var trimmed = line.Trim();
        if (trimmed == "-") return [];
        return [.. trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)];
    }
}