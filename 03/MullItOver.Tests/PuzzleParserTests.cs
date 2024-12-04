namespace MullItOver.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _parser = new();
  
  [Test]
  public async Task Parse_WhenInputIsEmpty_ReturnsEmptyList()
  {
    var result = _parser.Parse("");
    
    await Assert.That(result).IsEquivalentTo(new List<Instruction>());
  }
}