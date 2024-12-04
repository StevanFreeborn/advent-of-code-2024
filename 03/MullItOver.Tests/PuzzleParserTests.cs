namespace MullItOver.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _parser = new();
  private const string TestInput = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
  private const string Part2TestInput = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
  
  [Test]
  public async Task Parse_WhenInputIsEmpty_ReturnsEmptyList()
  {
    var result = _parser.Parse("");
    
    await Assert.That(result).IsEquivalentTo(new List<Instruction>());
  }

  [Test]
  public async Task Parse_WhenGivenSetOfMalformedInstructions_ItShouldReturnListOfActualInstructions()
  {
    var result = _parser.Parse(TestInput);

    await Assert.That(result).IsEquivalentTo(new List<Instruction>()
    {
      new MulInstruction(2, 4),
      new MulInstruction(5, 5),
      new MulInstruction(11, 8),
      new MulInstruction(8, 5),
    });
  }
  
  [Test]
  public async Task ParseWithConditionals_WhenInputIsEmpty_ReturnsEmptyList()
  {
    var result = _parser.ParseWithConditionals("");
    
    await Assert.That(result).IsEquivalentTo(new List<Instruction>());
  }

  [Test]
  public async Task ParseWithConditionals_WhenGivenSetOfMalformedInstructionsItShouldReturnListOfActualInstructions()
  {
    var result = _parser.ParseWithConditionals(Part2TestInput);

    await Assert.That(result).IsEquivalentTo(new List<Instruction>()
    {
      new MulInstruction(2, 4),
      new DontInstruction(),
      new MulInstruction(5, 5),
      new MulInstruction(11, 8),
      new DoInstruction(),
      new MulInstruction(8, 5),
    });
  }
}