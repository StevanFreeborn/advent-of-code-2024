namespace RedNosedReports.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _puzzleParser = new();

  [Test]
  public async Task Parse_WhenGivenInput_ItShouldReturnListOfReports()
  {
    string[] input = [
      "7 6 4 2 1",
      "1 2 7 8 9",
      "9 7 6 2 1",
      "1 3 2 4 5",
      "8 6 4 4 1",
      "1 3 6 7 9",
    ];

    var result = _puzzleParser.Parse(input);

    await Assert.That(result).IsEquivalentTo(new List<Report>()
    {
      new([7, 6, 4, 2, 1]),
      new([1, 2, 7, 8, 9]),
      new([9, 7, 6, 2, 1]),
      new([1, 3, 2, 4, 5]),
      new([8, 6, 4, 4, 1]),
      new([1, 3, 6, 7, 9]),
    });
  }
}