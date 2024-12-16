using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

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
var input = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));

var stopwatch = new Stopwatch();
stopwatch.Start();

var inputParts = input.Split($"{Environment.NewLine}{Environment.NewLine}");

var mapLines = inputParts[0].Split(Environment.NewLine);

var directions = inputParts[1]
  .Where(character => character.ToString() != Environment.NewLine && character.ToString() != string.Empty)
  .Select(Direction.From)
  .ToList();

var result = WarehouseMap.From(mapLines)
  .MoveRobot(directions)
  .CalculateTotalBoxGpsCoordinates();

stopwatch.Stop();
Console.WriteLine($"The sum of all boxes' GPS coordinates is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

class WarehouseMap
{
  private const char Robot = '@';
  private const char Wall = '#';
  private const char Box = 'O';
  private const char Empty = '.';
  
  private readonly Dictionary<Position, char> _positions;
  private Position RobotPosition => _positions.First(kvp => kvp.Value is Robot).Key;

  private WarehouseMap(Dictionary<Position, char> positions)
  {
    _positions = positions;
  }

  public static WarehouseMap From(string[] input)
  {
    var positions = new Dictionary<Position, char>();
    
    for (int y = 0; y < input.Length; y++)
    {
      for (int x = 0; x < input[0].Length; x++)
      {
        positions.Add(new(x, y), input[y][x]);
      }
    }

    return new(positions);
  }

  public WarehouseMap MoveRobot(List<Direction> directions)
  {
    var positions = _positions.ToDictionary();
    var robotPosition = RobotPosition;

    foreach (var direction in directions)
    {
      var nextPosition = robotPosition.GetNextPosition(direction);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);

      if (hasNextPosition is false)
      {
        continue;
      }
      
      if (nextPositionValue is Wall)
      {
        continue;
      }

      if (nextPositionValue is Empty)
      {
        positions[robotPosition] = Empty;
        positions[nextPosition] = Robot;
        continue;
      }

      if (nextPositionValue is Box && CanPushBox(nextPosition, direction, positions))
      {
        if (CanPushBox(nextPosition, direction, positions))
        {
          positions = PushBox(nextPosition, direction, positions);
          positions[robotPosition] = Empty;
          positions[nextPosition] = Robot;
        }
      }
    }
    
    return new(positions);
  }

  public int CalculateTotalBoxGpsCoordinates() => _positions
    .Where(kvp => kvp.Value is Box)
    .Select(kvp => kvp.Key)
    .Sum(p => p.GpsCoordinate);

  private static bool CanPushBox(Position boxPosition, Direction direction, Dictionary<Position, char> positions)
  {
    while (true)
    {
      var nextPosition = boxPosition.GetNextPosition(direction);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);

      if (hasNextPosition is false)
      {
        return false;
      }
      
      switch (nextPositionValue)
      {
        case Box:
          boxPosition = nextPosition;
          continue;
        case Empty:
          return true;
        default:
          return false;
      }
    }
  }

  private static Dictionary<Position, Char> PushBox(Position boxPosition, Direction direction, Dictionary<Position, char> currentPositions)
  {
    while (true)
    {
      var positions = currentPositions.ToDictionary();
      var nextPosition = boxPosition.GetNextPosition(direction);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);

      if (hasNextPosition is false)
      {
        return positions;
      }

      if (nextPositionValue is Box)
      {
        boxPosition = nextPosition;
        currentPositions = positions;
        continue;
      }

      positions[boxPosition] = Empty;
      positions[nextPosition] = Box;

      return positions;
    }
  }
}

record Position(int X, int Y)
{
  public int GpsCoordinate => (100 * Y) + X;
  
  public Position GetNextPosition(Direction direction)
  {
    return new(X + direction.Xd, Y + direction.Yd);
  }
}

record Direction(int Xd, int Yd)
{
  private const char UpCharacter = '^';
  private const char RightCharacter = '>';
  private const char DownCharacter = 'v';
  private const char LeftCharacter = '<';
  
  public static readonly Direction Up = new(0, -1);
  public static readonly Direction Down = new(0, 1);
  public static readonly Direction Left = new(-1, 0);
  public static readonly Direction Right = new(1, 0);

  public static Direction From(char character)
  {
    return character switch
    {
      UpCharacter => Up,
      RightCharacter => Right,
      DownCharacter => Down,
      LeftCharacter => Left,
      _ => throw new ArgumentException(@$"{nameof(character)} is not a valid: {character}"),
    };
  }
}