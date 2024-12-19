using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

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

// TODO: This still doesn't feel like the right API tho...even though it does solve
var stopwatch = new Stopwatch();
stopwatch.Start();

var width = 101;
var height = 103;
var times = isPart2 
  ? width * height 
  : 100;

var simulation = Simulation.From(width, height, input);
var simulationResults = simulation.Run(times);
var result = isPart2
  ? simulationResults.MinBy(kvp => kvp.Value).Key
  : simulation.SafetyFactor;

var msg = isPart2
  ? $"The Christmas tree appears after {result} seconds"
  : $"The safety factor will be {result}";

stopwatch.Stop();
Console.WriteLine($"{msg}. ({stopwatch.ElapsedMilliseconds}ms)");

if (isPart2)
{
  simulation.Run(result);
  simulation.PrintToConsole();
}

class Simulation
{
  private readonly List<Robot> _robots;
  private readonly int _width;
  private readonly int _height;
  private readonly List<Quadrant> _quadrants;
  private List<int> RobotsPerQuadrant =>
    _quadrants.Select(quadrant => _robots.Count(r => r.IsIn(quadrant))).ToList();
  public int SafetyFactor => RobotsPerQuadrant.Aggregate(1, (current, count) => current * count);

  private Simulation(int width, int height, List<Robot> robots)
  {
    _width = width;
    _robots = robots;
    _height = height;

    var middleColumn = _width / 2;
    var middleRow = _height / 2;
    
    var topLeftQuadrant = new Quadrant(0, middleColumn - 1, 0, middleRow - 1);
    var topRightQuadrant = new Quadrant(middleColumn + 1, _width - 1, 0, middleRow - 1);
    var bottomLeftQuadrant = new Quadrant(0, middleColumn - 1, middleRow + 1, _height - 1);
    var bottomRightQuadrant = new Quadrant(middleColumn + 1, _width - 1, middleRow + 1, _height - 1);
    _quadrants = [topLeftQuadrant, topRightQuadrant, bottomLeftQuadrant, bottomRightQuadrant];
  }

  public static Simulation From(int width, int height, string[] input) => new(width, height, input.Select(Robot.From).ToList());
  
  public Dictionary<int, int> Run(int times)
  {
    var safetyFactorMap = new Dictionary<int, int>();
    
    for (int i = 0; i < times; i++)
    {
      foreach (var robot in _robots)
      {
        robot.Move(_width, _height);
      }
      
      safetyFactorMap.Add(i + 1, SafetyFactor);
    }

    return safetyFactorMap;
  }

  public override string ToString()
  {
    var lines = new StringBuilder();
    
    for (var currentColumn = 0; currentColumn < _width; currentColumn++)
    {
      var row = new StringBuilder();
      
      for (int currentRow = 0; currentRow < _height; currentRow++)
      {
        row.Append(_robots.Any(r => r.IsIn(currentColumn, currentRow)) ? 'R' : '.');
      }

      lines.AppendLine(row.ToString());
    }

    return lines.ToString();
  }

  public void PrintToConsole() => Console.WriteLine(ToString());

  public Task PrintToFileAsync(string filename) =>
    File.WriteAllTextAsync(Path.Combine(AppContext.BaseDirectory, filename), ToString());
}

partial record Robot
{
  public int PositionX { get; private set; }
  public int PositionY { get; private set; }
  private int VelocityX { get; }
  private int VelocityY { get; }

  private Robot(int positionX, int positionY, int velocityX, int velocityY)
  {
    PositionX = positionX;
    PositionY = positionY;
    VelocityX = velocityX;
    VelocityY = velocityY;
  }

  public static Robot From(
    int positionX, 
    int positionY, 
    int velocityX, 
    int velocityY
  ) => new(positionX, positionY, velocityX, velocityY);
  
  public static Robot From(string input)
  {
    var matches = RobotRegex().Match(input);

    if (matches.Groups.Count is not 5)
    {
      throw new ArgumentException("The given robot input is missing values");
    }
    
    return new(
      int.Parse(matches.Groups[1].Value),
      int.Parse(matches.Groups[2].Value),
      int.Parse(matches.Groups[3].Value),
      int.Parse(matches.Groups[4].Value)
    );
  }

  public (int X, int Y) Move(int maxX, int maxY)
  {
    PositionX = (PositionX + VelocityX + maxX) % maxX;
    PositionY = (PositionY + VelocityY + maxY) % maxY;

    return (PositionX, PositionY);
  }

  public bool IsIn(Quadrant quadrant)
  {
    return PositionX >= quadrant.MinX &&
           PositionX <= quadrant.MaxX &&
           PositionY >= quadrant.MinY &&
           PositionY <= quadrant.MaxY;
  }

  public bool IsIn(int x, int y) => PositionX == x && PositionY == y;

  [GeneratedRegex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
  private static partial Regex RobotRegex();
}

record Quadrant(int MinX, int MaxX, int MinY, int MaxY);