using System.Diagnostics;

if (args.Length is 0)
{
  Console.WriteLine("Please provide a path to the input file.");
  return;
}

if (File.Exists(args[0]) is false)
{
  Console.WriteLine("The provided file does not exist.");
  return;
}

var isPart2 = args.Length is 2 && args[1] is "part2";
var input = await File.ReadAllLinesAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var result = new PuzzleParser().Parse(input).GetAntinodes(input, isPart2).Count;

stopwatch.Stop();
Console.WriteLine($"The number of antinode locations is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

// TODO: Make this better...it is ugly
static class AntennaGroupingsExtensions
{
  public static List<Point> GetAntinodes(this List<IGrouping<char,Antenna>> groupings, string[] input, bool isPart2 = false)
  {
    var antinodeLocations = new HashSet<Point>();

    foreach (var group in groupings)
    {
      var antennas = group.ToList();

      foreach (var a1 in antennas)
      {
        foreach (var a2 in antennas)
        {
          if (a1 == a2)
          {
            continue;
          }
          
          var pointOne = a1.ToPoint();
          var pointTwo = a2.ToPoint();
          var line = new Line(pointOne, pointTwo, input);
          var antinodes = isPart2 
            ? line.GetPart2Antinodes()
            : line.GetAntinodes();
          antinodes.ForEach(a => antinodeLocations.Add(a));
        }
      }
    }

    return antinodeLocations.ToList();
  }
}

class PuzzleParser
{
  public  List<IGrouping<char,Antenna>> Parse(string[] input)
  {
    var antennas = new List<Antenna>();

    foreach (var (line, rowIndex) in input.Select((line, index) => (line, index)))
    {
      foreach (var (character, colIndex) in line.Select((character, index) => (character, index)))
      {
        if (char.IsDigit(character) || char.IsLetter(character))
        {
          antennas.Add(new(rowIndex, colIndex, character));
        }
      }
    }
    
    var antennaGroupedByFrequency = antennas.GroupBy(a => a.Frequency).ToList();
    
    return antennaGroupedByFrequency;
  }
}

record Antenna(int RowIndex, int ColIndex, char Frequency)
{
  public Point ToPoint()
  {
    return new(RowIndex, ColIndex);
  }
}

record Point(int RowIndex, int ColIndex)
{
  public bool IsOnMap(string[] map)
  {
    return RowIndex > -1 &&
           RowIndex < map.Length &&
           ColIndex > -1 &&
           ColIndex < map[0].Length;
  }
}

record Slope(int Rise, int Run);

class Line(Point start, Point end, string[] Map)
{
  private readonly Slope _slope = new(end.RowIndex - start.RowIndex, end.ColIndex - start.ColIndex);
  
  public List<Point> GetAntinodes()
  {
    var doubledRun = _slope.Run * 2;
    var doubledRise = _slope.Rise * 2;
    
    return new List<Point>()
    {
      new(end.RowIndex - doubledRise, end.ColIndex - doubledRun),
      new(start.RowIndex + doubledRise, start.ColIndex + doubledRun),
    }.Where(p => p.IsOnMap(Map)).ToList();
  }

  public List<Point> GetPart2Antinodes()
  {
    var antinodes = new List<Point>();
    
    // we are finding antinodes from
    // start antenna back
    var currentPoint = start;

    while (currentPoint.IsOnMap(Map))
    {
      antinodes.Add(currentPoint);
      currentPoint = new(currentPoint.RowIndex - _slope.Rise, currentPoint.ColIndex - _slope.Run);
    }
    
    // we are finding antinodes from
    // end antenna out
    currentPoint = end;

    while (currentPoint.IsOnMap(Map))
    {
      antinodes.Add(currentPoint);
      currentPoint = new(currentPoint.RowIndex + _slope.Rise, currentPoint.ColIndex + _slope.Run);
    }

    return antinodes;
  }
}