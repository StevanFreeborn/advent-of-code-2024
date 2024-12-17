namespace ReindeerMaze.Tests;

public class PuzzleSolverTests
{
  [Test]
  [MethodDataSource(nameof(SolveTestCases))]
  public async Task Solve_WhenCalledWithInput_ItShouldReturnExpectedValue(SolveTestCase testCase)
  {
    var result = PuzzleSolver.Solve(testCase.Input);

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
      11048
    );
  }

  public record SolveTestCase(string[] Input, int ExpectedValue);
}