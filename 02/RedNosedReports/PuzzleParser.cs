namespace RedNosedReports;

class PuzzleParser
{
  public List<Report> Parse(string[] lines)
  {
    var reports = new List<Report>();

    foreach (var line in lines)
    {
      var numbers = line.Split(' ').Select(int.Parse).ToList();
      reports.Add(new Report(numbers));
    }

    return reports;
  }
}
