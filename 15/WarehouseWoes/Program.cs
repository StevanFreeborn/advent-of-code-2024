using System.Diagnostics;
using System.Text;

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
var input = await File.ReadAllTextAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var (map, directions) = PuzzleParser.Parse(input);
var result = map.MoveRobot(directions).CalculateTotalBoxGpsCoordinates();

stopwatch.Stop();
Console.WriteLine($"The sum of all boxes' GPS coordinates is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

static class PuzzleParser
{
  public static (WarehouseMap Map, List<Direction> Directions) Parse(string input)
  {
    var inputParts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
    
    var map = WarehouseMap.From(inputParts[0].Split(Environment.NewLine));
    
    var directions = inputParts[1]
      .Where(character =>
      {
        var str = character.ToString();
        return str != "\r" && str != "\n" && str != "\r\n";
      })
      .Select(Direction.From)
      .ToList();
    
    return (map, directions);
  }
}

class WarehouseMap
{
  private const char Robot = '@';
  private const char Wall = '#';
  private const char Box = 'O';
  private const char Empty = '.';
  
  public readonly Dictionary<Position, char> Positions;
  
  private WarehouseMap(Dictionary<Position, char> positions)
  {
    Positions = positions;
  }

  public static WarehouseMap From(string[] input)
  {
    var positions = new Dictionary<Position, char>();
    
    for (var y = 0; y < input.Length; y++)
    {
      for (var x = 0; x < input[0].Length; x++)
      {
        positions.Add(new(x, y), input[y][x]);
      }
    }

    return new(positions);
  }

  public WarehouseMap MoveRobot(List<Direction> directions)
  {
    var positions = Positions.ToDictionary();
    
    foreach (var direction in directions)
    {
      var robotPosition = positions.First(kvp => kvp.Value is Robot).Key;
      var nextPosition = robotPosition.GetNextPosition(direction);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);

      if (hasNextPosition is false)
      {
        continue;
      }
      
      switch (nextPositionValue)
      {
        case Wall:
          continue;
        case Empty:
          positions[robotPosition] = Empty;
          positions[nextPosition] = Robot;
          continue;
        case Box when CanPushBox(nextPosition, direction, positions):
          positions = PushBox(nextPosition, direction, positions);
          positions[robotPosition] = Empty;
          positions[nextPosition] = Robot;
          break;
      }
    }
    
    return new(positions);
  }

  public int CalculateTotalBoxGpsCoordinates()
  {
    var boxes = Positions.Where(kvp => kvp.Value is Box).ToList();
    return boxes.Select(kvp => kvp.Key).Sum(p => p.GpsCoordinate);
  }

  public override string ToString()
  {
    var maxX = Positions.Select(kvp => kvp.Key).Max(p => p.X);
    var maxY = Positions.Select(kvp => kvp.Key).Max(p => p.Y);

    var map = new StringBuilder();

    for (var y = 0; y <= maxY; y++)
    {
      var row = new StringBuilder();
      
      for (var x = 0; x <= maxX; x++)
      {
        var value = Positions[new(x, y)];
        row.Append(value);
      }

      map.AppendLine(row.ToString());
    }

    return map.ToString();
  }

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
    var positions = currentPositions.ToDictionary();
    
    while (true)
    {
      var nextPosition = boxPosition.GetNextPosition(direction);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);

      if (hasNextPosition is false)
      {
        return positions;
      }

      if (nextPositionValue is Box)
      {
        boxPosition = nextPosition;
        continue;
      }
      
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

  private static readonly Direction Up = new(0, -1);
  private static readonly Direction Down = new(0, 1);
  private static readonly Direction Left = new(-1, 0);
  private static readonly Direction Right = new(1, 0);

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