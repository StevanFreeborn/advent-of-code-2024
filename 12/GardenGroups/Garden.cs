namespace GardenGroups;

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
    var positionsToVisit = new Queue<Position>();
    
    positionsToVisit.Enqueue(position);

    while (positionsToVisit.Count is not 0)
    {
      var currentPosition = positionsToVisit.Dequeue();
      
      if (visitedPositions.Contains(currentPosition))
      {
        continue;
      }
      
      region.AddPosition(currentPosition);
      visitedPositions.Add(currentPosition);

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