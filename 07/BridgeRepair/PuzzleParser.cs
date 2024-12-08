namespace BridgeRepair;

class PuzzleParser
{
  public List<Equation> Parse(string[] input)
  {
    return input.Select(s => s.ToEquation()).ToList();
  }
}