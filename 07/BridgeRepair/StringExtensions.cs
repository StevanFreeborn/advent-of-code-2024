namespace BridgeRepair;

static class StringExtensions
{
  public static Equation ToEquation(this string line)
  {
    var parts = line.Split(':');
    var testValue = long.Parse(parts[0]);
    var numbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries)
      .Select(long.Parse)
      .ToList();
    return new(testValue, numbers);
  }
}