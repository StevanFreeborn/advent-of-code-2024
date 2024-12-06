namespace PrintQueue.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _puzzleParser = new();
  private readonly string _exampleInput = $"47|53{Environment.NewLine}97|13{Environment.NewLine}97|61{Environment.NewLine}97|47{Environment.NewLine}75|29{Environment.NewLine}61|13{Environment.NewLine}75|53{Environment.NewLine}29|13{Environment.NewLine}97|29{Environment.NewLine}53|29{Environment.NewLine}61|53{Environment.NewLine}97|53{Environment.NewLine}61|29{Environment.NewLine}47|13{Environment.NewLine}75|47{Environment.NewLine}97|75{Environment.NewLine}47|61{Environment.NewLine}75|61{Environment.NewLine}47|29{Environment.NewLine}75|13{Environment.NewLine}53|13{Environment.NewLine}{Environment.NewLine}75,47,61,53,29{Environment.NewLine}97,61,53,29,13{Environment.NewLine}75,29,13{Environment.NewLine}75,97,47,61,53{Environment.NewLine}61,13,29{Environment.NewLine}97,13,75,29,47";

  private static async Task<string> GetPuzzleInput()
  {
    return await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task Parse_WhenGivenExampleInput_ItShouldReturnExpectedRules()
  {
    var result = _puzzleParser.Parse(_exampleInput);

    await Assert.That(result.Rules).IsEquivalentTo(new List<OrderRule>()
    {
      new(47, 53),
      new(97, 13),
      new(97, 61),
      new(97, 47),
      new(75, 29),
      new(61, 13),
      new(75, 53),
      new(29, 13),
      new(97, 29),
      new(53, 29),
      new(61, 53),
      new(97, 53),
      new(61, 29),
      new(47, 13),
      new(75, 47),
      new(97, 75),
      new(47, 61),
      new(75, 61),
      new(47, 29),
      new(75, 13),
      new(53, 13),
    });
  }
  
  [Test]
  public async Task Parse_WhenGivenExampleInput_ItShouldReturnExpectedUpdates()
  {
    var result = _puzzleParser.Parse(_exampleInput);

    await Assert.That(result.Updates).IsEquivalentTo(new List<Update>()
    {
      new([75,47,61,53,29]),
      new([97,61,53,29,13]),
      new([75,29,13]),
      new([75,97,47,61,53]),
      new([61,13,29]),
      new([97,13,75,29,47]),
    });
  }

  [Test]
  public async Task Parse_WhenGivenPuzzleInput_ItShouldReturnExpectedResult()
  {
    var input = await GetPuzzleInput();

    var parseResult = _puzzleParser.Parse(input);
    
    var validator = new UpdateValidator(parseResult.Rules);

    var result = parseResult.Updates
      .Where(u => validator.Validate(u))
      .Select(u => u.GetMiddlePage())
      .Sum();

    await Assert.That(result).IsEqualTo(6260);
  }
}