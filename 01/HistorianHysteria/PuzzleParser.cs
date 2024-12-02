namespace HistorianHysteria;

class PuzzleParser
{
  public (List<int> Left, List<int> Right) Parse(string[] lines)
  {
    var left = new List<int>();
    var right = new List<int>();

    foreach (var line in lines)
    {
      var numbers = line.Split()
        .Where(s => string.IsNullOrWhiteSpace(s) is false)
        .Select(int.Parse)
        .ToArray();
      
      left.Add(numbers[0]);
      right.Add(numbers[1]);
    }

    return (left, right);
  }
}