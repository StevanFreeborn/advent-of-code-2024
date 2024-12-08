namespace BridgeRepair.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _parser = new();
  private readonly string[] _exampleInput = [
    "190: 10 19",
    "3267: 81 40 27",
    "83: 17 5",
    "156: 15 6",
    "7290: 6 8 6 15",
    "161011: 16 10 13",
    "192: 17 8 14",
    "21037: 9 7 18 13",
    "292: 11 6 16 20",
  ];

  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task SolutionPartOne_WhenGivenExampleInput_ItShouldReturnExpectedValue()
  {
    var result = _parser.Parse(_exampleInput).Where(e => e.IsPossible()).Sum(e => e.TestValue);

    await Assert.That(result).IsEqualTo(3749);
  }
  
  [Test]
  public async Task SolutionPartOne_WhenGivenPuzzleInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();
    var result = _parser.Parse(input).Where(e => e.IsPossible()).Sum(e => e.TestValue);

    await Assert.That(result).IsEqualTo(14711933466277);
  }
  
  [Test]
  public async Task SolutionPartTwo_WhenGivenExampleInput_ItShouldReturnExpectedValue()
  {
    var result = _parser.Parse(_exampleInput).Where(e => e.IsPossible(isPart2: true)).Sum(e => e.TestValue);

    await Assert.That(result).IsEqualTo(11387);
  }
  
  [Test]
  public async Task SolutionPartTwo_WhenGivenPuzzleInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();
    var result = _parser.Parse(input).Where(e => e.IsPossible(isPart2: true)).Sum(e => e.TestValue);

    await Assert.That(result).IsEqualTo(286580387663654);
  }
  
  [Test]
  public async Task Parse_WhenGivenExampleInput_ItShouldReturnExpectedList()
  {
    var result = _parser.Parse(_exampleInput);

    await Assert.That(result).IsEquivalentTo(new List<Equation>()
    {
      new(190, [10, 19]),
      new(3267, [81, 40, 27]),
      new(83, [17, 5]),
      new(156, [15, 6]),
      new(7290, [6, 8, 6, 15]),
      new(161011, [16, 10, 13]),
      new(192, [17, 8, 14]),
      new(21037, [9, 7, 18, 13]),
      new(292, [11, 6, 16, 20]),
    });
  }
}