namespace HoofIt;

class TopoMap
{
  private static readonly Direction[] Directions = [
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

  public List<int> GetTrailheadRatings()
  {
    var ratings = new List<int>();
    
    WalkMap((x, y) =>
    {
      var hasHeight = int.TryParse(_grid[y][x].ToString(), out var currentHeight);

      if (hasHeight is false || currentHeight is not 0)
      {
        return;
      }

      var rating = GetPathsForTrail(new(x, y, currentHeight));
      ratings.Add(rating);
    });

    return ratings;
  }
  
  public List<int> GetTrailheadScores()
  {
    var trailScores = new List<int>();
    
    WalkMap((x, y) =>
    {
      var hasHeight = int.TryParse(_grid[y][x].ToString(), out var currentHeight);

      if (hasHeight is false || currentHeight is not 0)
      {
        return;
      }

      var score = GetPathsForTrail(new(x, y, currentHeight), needToBeUnique: true);
      trailScores.Add(score);
    });

    return trailScores;
  }

  private int GetPathsForTrail(Position trailhead, bool needToBeUnique = false)
  {
    var trailheadScore = 0;
    var visitedPositions = new HashSet<Position>();
    var positionsToVisit = new Queue<Position>();
    
    positionsToVisit.Enqueue(trailhead);

    while (positionsToVisit.Count is not 0)
    {
      var currentPosition = positionsToVisit.Dequeue();
      
      if (needToBeUnique && visitedPositions.Contains(currentPosition))
      {
        continue;
      }

      visitedPositions.Add(currentPosition);
      
      if (currentPosition.Height is 9)
      {
        trailheadScore++;
        continue;
      }

      foreach (var direction in Directions)
      {
        if (currentPosition.TryGetNextPosition(direction, _grid, out var newPosition) is false)
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
  
  private void WalkMap(Action<int, int> callback)
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