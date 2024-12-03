namespace RedNosedReports;

class PuzzleParser
{
  public List<Report> Parse(string[] lines) => lines
    .Select(line => line.Split(' ').Select(int.Parse).ToList())
    .Select(numbers => new Report(numbers))
    .ToList();
}
