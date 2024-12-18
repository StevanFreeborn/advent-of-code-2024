namespace ReindeerMaze.Tests;

public class PuzzleSolverTests
{
  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task PartOneSolution_WhenCalledWithInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();
    
    var result = PuzzleSolver.Solve(input);

    await Assert.That(result).IsEqualTo(90460);
  }
  
  [Test]
  [MethodDataSource(nameof(SolveTestCases))]
  public async Task Solve_WhenCalledWithInput_ItShouldReturnExpectedValue(SolveTestCase testCase)
  {
    var result = PuzzleSolver.Solve(testCase.Input, testCase.IsPart2);

    await Assert.That(result).IsEqualTo(testCase.ExpectedValue);
  }

  public static IEnumerable<Func<SolveTestCase>> SolveTestCases()
  {
    yield return () => new(
      [
        "###############",
        "#.......#....E#",
        "#.#.###.#.###.#",
        "#.....#.#...#.#",
        "#.###.#####.#.#",
        "#.#.#.......#.#",
        "#.#.#####.###.#",
        "#...........#.#",
        "###.#.#####.#.#",
        "#...#.....#.#.#",
        "#.#.#.###.#.#.#",
        "#.....#...#.#.#",
        "#.###.#.#.#.#.#",
        "#S..#.....#...#",
        "###############",
      ],
      false,
      7036
    );

    yield return () => new(
      [
        "#################",
        "#...#...#...#..E#",
        "#.#.#.#.#.#.#.#.#",
        "#.#.#.#...#...#.#",
        "#.#.#.#.###.#.#.#",
        "#...#.#.#.....#.#",
        "#.#.#.#.#.#####.#",
        "#.#...#.#.#.....#",
        "#.#.#####.#.###.#",
        "#.#.#.......#...#",
        "#.#.###.#####.###",
        "#.#.#...#.....#.#",
        "#.#.#.#####.###.#",
        "#.#.#.........#.#",
        "#.#.#.#########.#",
        "#S#.............#",
        "#################",
      ],
      false,
      11048
    );
    
    yield return () => new(
      [
        "###############",
        "#.......#....E#",
        "#.#.###.#.###.#",
        "#.....#.#...#.#",
        "#.###.#####.#.#",
        "#.#.#.......#.#",
        "#.#.#####.###.#",
        "#...........#.#",
        "###.#.#####.#.#",
        "#...#.....#.#.#",
        "#.#.#.###.#.#.#",
        "#.....#...#.#.#",
        "#.###.#.#.#.#.#",
        "#S..#.....#...#",
        "###############",
      ],
      true,
      45
    );

    yield return () => new(
      [
        "#################",
        "#...#...#...#..E#",
        "#.#.#.#.#.#.#.#.#",
        "#.#.#.#...#...#.#",
        "#.#.#.#.###.#.#.#",
        "#...#.#.#.....#.#",
        "#.#.#.#.#.#####.#",
        "#.#...#.#.#.....#",
        "#.#.#####.#.###.#",
        "#.#.#.......#...#",
        "#.#.###.#####.###",
        "#.#.#...#.....#.#",
        "#.#.#.#####.###.#",
        "#.#.#.........#.#",
        "#.#.#.#########.#",
        "#S#.............#",
        "#################",
      ],
      true,
      64
    );
  }

  public record SolveTestCase(string[] Input, bool IsPart2, int ExpectedValue);
}