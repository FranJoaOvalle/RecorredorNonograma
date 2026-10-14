using System.Text;
using RecorredorNonograma.IO;
using RecorredorNonograma.Search;
using RecorredorNonograma.Validation;

Console.OutputEncoding = Encoding.UTF8;

string[] puzzleFiles =
[
    "Puzzles/cross5.txt",
    "Puzzles/test10.txt",
    "Puzzles/test15.txt"
];

if (args.Length != 1)
{
    PrintUsage();
    return;
}

try
{
    if (args[0] == "--benchmark")
        RunBenchmarks(puzzleFiles);
    else
        SolvePuzzle(args[0]);
}
catch (Exception ex) when (
    ex is IOException or ArgumentException or FormatException or InvalidOperationException)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.ExitCode = 1;
}
return;

static void SolvePuzzle(string path)
{
    var puzzle = NonogramFileLoader.Load(path);
    var solver = new NonogramSolver(puzzle);
    var solution = solver.Solve();
    
    Console.WriteLine($"Tablero: {puzzle.Width}×{puzzle.Height}");
    Console.WriteLine($"Tiempo: {solver.ElapsedTime.TotalMilliseconds:F4} ms");
    Console.WriteLine($"Memoria asignada: {solver.AllocatedBytes:N0} bytes");
    Console.WriteLine($"Nodos expandidos: {solver.NodesExpanded}");
    
    if (solution is null)
    {
        Console.WriteLine("No se encontró una solución.");
        return;
    }
    
    if (!NonogramSolutionValidator.IsValid(puzzle, solution))
        throw new InvalidOperationException("La solución encontrada es inválida.");
    
    Console.WriteLine("\n¡Solución válida encontrada!\n");
    
    for (var row = 0; row < solution.Height; row++)
    {
        for (var column = 0; column < solution.Width; column++)
            Console.Write(solution.GetCell(row, column) ? '⬛' : '⬜');
        
        Console.WriteLine();
    }
}

static void RunBenchmarks(string[] files)
{
    const int repetitions = 30;
    const int warmup = 5;
    
    foreach (var file in files)
    {
        var puzzle = NonogramFileLoader.Load(file);
        var solver = new NonogramSolver(puzzle);
        
        for (var i = 0; i < warmup; i++)
            solver.Solve();
        
        double totalTime = 0;
        long totalMemory = 0;
        
        for (var i = 0; i < repetitions; i++)
        {
            var solution = solver.Solve();
            
            if (solution is null || !NonogramSolutionValidator.IsValid(puzzle, solution))
                throw new InvalidOperationException($"No se encontró una solución válida para {file}.");
            
            totalTime += solver.ElapsedTime.TotalMilliseconds;
            totalMemory += solver.AllocatedBytes;
        }
        
        Console.WriteLine($"=== {Path.GetFileName(file)} ({puzzle.Width}×{puzzle.Height}) ===");
        Console.WriteLine($"Repeticiones: {repetitions}");
        Console.WriteLine($"Tiempo promedio: {totalTime / repetitions:F4} ms");
        Console.WriteLine($"Memoria promedio: {totalMemory / repetitions:N0} bytes");
        Console.WriteLine($"Nodos expandidos: {solver.NodesExpanded}");
        Console.WriteLine();
    }
}

static void PrintUsage()
{
    Console.WriteLine("Uso:");
    Console.WriteLine("  dotnet run -c Release -- <archivo>");
    Console.WriteLine("  dotnet run -c Release -- --benchmark");
}