namespace MullItOver.Tests;

public class PuzzleInputTests
{
  private readonly PuzzleParser _parser = new();

  private async Task<string> GetPuzzleInput()
  {
    var inputPath = Path.Combine(AppContext.BaseDirectory, "INPUT.txt");
    return await File.ReadAllTextAsync(inputPath);
  }

  [Test]
  public async Task Solve_WhenGivenInput_ItShouldSolvePartOne()
  {
    var input = await GetPuzzleInput();
    var result = _parser.Parse(input).Execute();

    await Assert.That(result).IsEqualTo(174960292);
  }
  
  [Test]
  public async Task Solve_WhenGivenInput_ItShouldSolvePartTwo()
  {
    var input = await GetPuzzleInput();
    var result = _parser.ParseWithConditionals(input).Execute();

    await Assert.That(result).IsEqualTo(56275602);
  }
}