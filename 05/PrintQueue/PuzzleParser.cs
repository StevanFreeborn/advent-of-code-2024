namespace PrintQueue;

class PuzzleParser
{
  public ParseResult Parse(string input)
  {
    var inputParts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
    var rules = ParseRules(inputParts[0]);
    var updates = ParseUpdates(inputParts[1]);
    return new ParseResult(rules, updates);
  }

  private static List<Update> ParseUpdates(string input)
  {
    return input.Split(Environment.NewLine)
      .Select(s => new Update(
        s.Split(",").Select(int.Parse).ToList()
      ))
      .ToList();
  }
  
  private static List<OrderRule> ParseRules(string input)
  {
    return input
      .Split(Environment.NewLine)
      .Select(s =>
      {
        var parts = s.Split("|");
        return new OrderRule(
          int.Parse(parts[0]),
          int.Parse(parts[1])
        );
      })
      .ToList();
  }
}