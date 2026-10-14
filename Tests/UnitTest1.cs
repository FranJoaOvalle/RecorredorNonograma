using RecorredorNonograma.IO;
using RecorredorNonograma.Models;
using RecorredorNonograma.Search;
using RecorredorNonograma.Validation;
using Xunit;

namespace RecorredorNonograma.Tests;

public class UnitTest1
{
    [Fact]
    public void Solve_1x1Filled_ReturnsValidSolution()
    {
        var puzzle = new NonogramPuzzle(1, 1, [[1]], [[1]]);
        var solver = new NonogramSolver(puzzle);
        
        var solution = solver.Solve();
        
        Assert.NotNull(solution);
        Assert.True(solution.GetCell(0, 0));
        Assert.True(NonogramSolutionValidator.IsValid(puzzle, solution));
    }
    
    [Fact]
    public void Solve_1x1Empty_ReturnsValidSolution()
    {
        var puzzle = new NonogramPuzzle(1, 1, [[]], [[]]);
        var solver = new NonogramSolver(puzzle);
        
        var solution = solver.Solve();
        
        Assert.NotNull(solution);
        Assert.False(solution.GetCell(0, 0));
        Assert.True(NonogramSolutionValidator.IsValid(puzzle, solution));
    }
    
    [Fact]
    public void Solve_IncompatibleClues_ReturnsNull()
    {
        var puzzle = new NonogramPuzzle(1, 1, [[1]], [[]]);
        var solver = new NonogramSolver(puzzle);
        
        Assert.Null(solver.Solve());
    }

    [Fact]
    public void Solve_RectangularPuzzle_ReturnsValidSolution()
    {
        var puzzle = new NonogramPuzzle(
            3, 2,
            [[2], [1]],
            [[1], [2], []]);
        
        var solver = new NonogramSolver(puzzle);
        var solution = solver.Solve();
        
        Assert.NotNull(solution);
        Assert.True(NonogramSolutionValidator.IsValid(puzzle, solution));
    }
    
    [Fact]
    public void Generate_MultipleBlocks_ReturnsExpectedPatterns()
    {
        var patterns = RowPatternGenerator.Generate(5, [2, 1]);
        
        var results = patterns.Select(pattern =>
            new string(pattern.Select(cell => cell ? '#' : '.').ToArray())
        ).ToArray();
        
        Assert.Equal(["##.#.", "##..#", ".##.#"], results);
    }
    
    [Fact]
    public void Generate_EmptyClues_ReturnsEmptyRow()
    {
        var patterns = RowPatternGenerator.Generate(5, []);
        
        Assert.Single(patterns);
        Assert.All(patterns[0], Assert.False);
    }

    [Fact]
    public void Generate_ImpossibleClues_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            RowPatternGenerator.Generate(5, [3, 3]));
    }
    
    [Fact]
    public void Puzzle_InvalidDimensions_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new NonogramPuzzle(0, 5, [], []));
    }
    
    [Fact]
    public void FileLoader_ValidFile_LoadsPuzzle()
    {
        var path = Path.GetTempFileName();
        
        try
        {
            File.WriteAllText(path, "2 1\nROWS\n2\nCOLUMNS\n1\n1");
            
            var puzzle = NonogramFileLoader.Load(path);
            
            Assert.Equal(2, puzzle.Width);
            Assert.Equal(1, puzzle.Height);
            
            var solution = new NonogramSolver(puzzle).Solve();
            
            Assert.NotNull(solution);
            Assert.True(NonogramSolutionValidator.IsValid(puzzle, solution));
        }
        finally
        {
            File.Delete(path);
        }
    }
    
    [Fact]
    public void FileLoader_InvalidHeader_ThrowsException()
    {
        var path = Path.GetTempFileName();
        
        try
        {
            File.WriteAllText(path, "2 1\nROWS\n2\nINVALID\n1\n1");
            
            Assert.Throws<FormatException>(() =>
                NonogramFileLoader.Load(path));
        }
        finally
        {
            File.Delete(path);
        }
    }
}