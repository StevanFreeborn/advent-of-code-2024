namespace HistorianHysteria.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _puzzleParser = new();

  [Test]
  public async Task Parse_WhenGivenLines_ReturnsLeftAndRightLists()
  {
    var input = new string[] 
    {
      "3   4",
      "4   3",
      "2   5",
      "1   3",
      "3   9",
      "3   3",
    };

    var (left, right) = _puzzleParser.Parse(input);

    await Assert.That(left).IsEquivalentTo(new List<int>() { 3, 4, 2, 1, 3, 3 });
    await Assert.That(right).IsEquivalentTo(new List<int>() { 4, 3, 5, 3, 9, 3 });
  }
}
