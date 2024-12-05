namespace CeresSearch.Tests;

public class GridTests
{
  private readonly string[] _exampleInput = [
    "MMMSXXMASM",
    "MSAMXMSMSA",
    "AMXSXMAAMM",
    "MSAMASMSMX",
    "XMASAMXAMM",
    "XXAMMXXAMA",
    "SMSMSASXSS",
    "SAXAMASAAA",
    "MAMMMXMMMM",
    "MXMXAXMASX",
  ];

  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }

  [Test]
  public async Task CountOccurrences_WhenCalledWithExampleInput_ItShouldReturnExpectedCount()
  {
    var result = new Grid(_exampleInput).CountOccurrences("XMAS");

    await Assert.That(result).IsEqualTo(18);
  }
  
  [Test]
  public async Task CountOccurrences_WhenCalledWithPuzzleInput_ItShouldReturnExpectedCount()
  {
    var input = await GetPuzzleInput();
    var result = new Grid(input).CountOccurrences("XMAS");

    await Assert.That(result).IsEqualTo(2644);
  }

  [Test]
  public async Task CountXmases_WhenCalledWithExampleInput_ItShouldReturnExpectedCount()
  {
    var result = new Grid(_exampleInput).CountXmases();

    await Assert.That(result).IsEqualTo(9);
  }
  
  [Test]
  public async Task CountXmases_WhenCalledWithPuzzleInput_ItShouldReturnExpectedCount()
  {
    var input = await GetPuzzleInput();
    var result = new Grid(input).CountXmases();

    await Assert.That(result).IsEqualTo(1952);
  }
}