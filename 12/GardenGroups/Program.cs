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

var result = Garden.From(input).CalculateFencePriceBasedOnPerimeter();

stopwatch.Stop();
Console.WriteLine($"The total cost to fence the garden is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

class Garden
{
  private static readonly Direction[] Directions = [
    Direction.Up,
    Direction.Down,
    Direction.Left,
    Direction.Right,
  ];
  
  private readonly string[] _grid;

  private Garden(string[] grid)
  {
    _grid = grid;
  }

  public static Garden From(string[] input) => new(input);
  
  public int CalculateFencePriceBasedOnSides()
  {
    var visitedPositions = new HashSet<Position>();
    var total = 0;
    
    WalkGarden((x, y) =>
    {
      var currentPosition = new Position(x, y, _grid[y][x]);

      if (visitedPositions.Contains(currentPosition))
      {
        return;
      }
      
      var region = FindRegion(currentPosition, visitedPositions);
      total += region.CalculateFencePriceBasedOnSides();
    });

    return total;
  }
  
  public int CalculateFencePriceBasedOnPerimeter()
  {
    var visitedPositions = new HashSet<Position>();
    var total = 0;
    
    WalkGarden((x, y) =>
    {
      var currentPosition = new Position(x, y, _grid[y][x]);

      if (visitedPositions.Contains(currentPosition))
      {
        return;
      }
      
      var region = FindRegion(currentPosition, visitedPositions);
      total += region.CalculateFencePriceBasedOnPerimeter();
    });

    return total;
  }

  private Region FindRegion(Position position, HashSet<Position> visitedPositions)
  {
    var region = Region.From(position);
    var regionPositions = new HashSet<Position>();

    var positionsToVisit = new Queue<Position>();
    
    positionsToVisit.Enqueue(position);

    while (positionsToVisit.Count is not 0)
    {
      var currentPosition = positionsToVisit.Dequeue();
      
      if (visitedPositions.Contains(currentPosition))
      {
        continue;
      }
      
      regionPositions.Add(currentPosition);
      region.IncreaseArea();

      foreach (var direction in Directions)
      {
        if (
          currentPosition.TryGetNextPosition(direction, _grid, out var newPosition) is false ||
          newPosition.PlantType != currentPosition.PlantType
        )
        {
          region.IncreasePerimeter();
          continue;
        }
        
        positionsToVisit.Enqueue(newPosition);
      }
    }
    
    // TODO: Do something here to identify how many
    // sides each position contributes in a region
    
    return region;
  }
  
  private void WalkGarden(Action<int, int> callback)
  {
    for (var y = 0; y < _grid.Length; y++)
    {
      var row = _grid[y];

      for (var x = 0; x < row.Length; x++)
      {
        callback(x, y);
      }
    }
  }
}

class Region
{
  public char PlantType { get; }
  public int Area { get; private set; }
  public int Perimeter { get; private set; }
  public int Sides { get; private set; }

  private Region(char plantType)
  {
    PlantType = plantType;
  }

  public static Region From(char plantType) => new(plantType);
  public static Region From(Position position) => new(position.PlantType);
  
  public void IncreaseArea() => Area++;
  public void IncreasePerimeter() => Perimeter++;
  public void IncreaseSize() => Sides++;
  public int CalculateFencePriceBasedOnPerimeter() => Area * Perimeter;

  public int CalculateFencePriceBasedOnSides() => Area * Sides;
}

record Position(int X, int Y, char PlantType)
{
  public bool TryGetNextPosition(Direction direction, string[] grid, out Position nextPosition)
  {
    var x = X + direction.XD;
    var y = Y + direction.YD;

    nextPosition = new(x, y, '.');

    var isOffGrid = IsOffGrid(nextPosition, grid);

    if (isOffGrid)
    {
      return false;
    }
    
    nextPosition = nextPosition with { PlantType = grid[y][x] };
    return true;
  }
  
  private static bool IsOffGrid(Position position, string[] grid)
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