namespace RecorredorNonograma.Search;

public static class RowPatternGenerator
{
    public static List<bool[]> Generate(int width, int[] clues)
    {
        if (width <= 0) throw new ArgumentException("El ancho debe ser mayor a cero.");
        
        ArgumentNullException.ThrowIfNull(clues);
        
        if (clues.Any(value =>  value <= 0))
            throw new ArgumentException("Las longitudes de los bloques deben ser positivas.");
        
        var requiredSpace = clues.Sum(value => (long)value) + Math.Max(0, clues.Length - 1);
        if (requiredSpace > width)
            throw new ArgumentException("Las pistas exceden las dimensiones posibles.");
        
        List<bool[]> patterns = [];
        
        PlaceBlock(0, 0, new bool[width]);
        
        return patterns;
        
        void PlaceBlock(int clueIndex, int minStart, bool[] current)
        {
            if (clueIndex == clues.Length)
            {
                patterns.Add((bool[])current.Clone());
                return;
            }
            
            var spaceNeeded = 0;
            for (var i = clueIndex; i < clues.Length; i++) 
                spaceNeeded += clues[i];
            
            spaceNeeded += clues.Length - clueIndex - 1;
            
            var maxStart = width - spaceNeeded;
            var blockLength = clues[clueIndex];
            
            for (var startIndex = minStart; startIndex <= maxStart; startIndex++)
            {
                for (var i = 0; i < blockLength; i++) 
                    current[startIndex + i] = true;
                
                PlaceBlock(clueIndex + 1, startIndex + blockLength + 1, current);
                
                for (var i = 0; i < blockLength; i++) 
                    current[startIndex + i] = false;
            }
        }
    }
}