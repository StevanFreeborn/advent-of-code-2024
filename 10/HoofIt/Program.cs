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

var map = TopoMap.From(input);

// i'm going to to walk over the grid
// i'm going to only worry about places where
// there is a zero

// need to check if we are on a nine
// before we explore...also we should
// be able to add to score here

// when i encounter a zero i need to begin
// exploring nearby tiles that increment by 1
// in each left, right, up, down direction
// any adj tile that fits this...is a tile i
// need to also explore

stopwatch.Stop();
Console.WriteLine($". ({stopwatch.ElapsedMilliseconds}ms)");

class TopoMap
{
  private static Direction[] _directions = [
    Direction.Up,
    Direction.Down,
    Direction.Left,
    Direction.Right,
  ];
  
  private readonly string[] _grid;

  private TopoMap(string[] input)
  {
    _grid = input;
  }

  public static TopoMap From(string[] input) => new(input);

  // TODO: This actually needs to be GetTrailHeadScore
  // I want to perform this search when I know I've reached
  // a trail head.
  public int GetTrailHeadScores()
  {
    var trailheadScore = 0;
    var visitedPositions = new HashSet<Position>();
    var positionsToVisit = new Queue<Position>();

    var startingX = 0;
    var startingY = 0;
    var startingHeight = int.Parse(_grid[startingY][startingX].ToString());
    positionsToVisit.Enqueue(new(startingX, startingY, startingHeight));

    while (positionsToVisit.Count != 0)
    {
      var currentPosition = positionsToVisit.Dequeue();
      
      if (visitedPositions.Contains(currentPosition))
      {
        continue;
      }

      visitedPositions.Add(currentPosition);
      
      if (currentPosition.Height is 9)
      {
        trailheadScore++;
        continue;
      }

      foreach (var direction in _directions)
      {
        if (currentPosition.TryGetNextPosition(direction, _grid, out var newPosition) is false)
        {
          continue;
        }

        if (visitedPositions.Contains(newPosition))
        {
          continue;
        }

        var heightDifference = newPosition.Height - currentPosition.Height;

        if (heightDifference is not 1)
        {
          continue;
        }
        
        positionsToVisit.Enqueue(newPosition);
      }
    }
    
    return trailheadScore;
  }
}

record Position(int X, int Y, int Height)
{
  public bool TryGetNextPosition(Direction direction, string[] grid, out Position position)
  {
    var x = X + direction.XD;
    var y = Y + direction.YD;

    position = new(x, y, -1);

    var isOffGrid = IsOffGrid(position, grid);

    if (isOffGrid)
    {
      return false;
    }

    position = position with { Height = grid[y][x] };
    return true;
  }
  
  private bool IsOffGrid(Position position, string[] grid)
  {
    return position.X < 0 ||
           position.X > grid[0].Length - 1 ||
           position.Y > grid.Length - 1 ||
           position.Y < 0;
  }
}

record Direction(int XD, int YD)
{
  public static Direction Up = new(0, -1);
  public static Direction Down = new(0, 1);
  public static Direction Left = new(-1, 0);
  public static Direction Right = new(1, 0);
}