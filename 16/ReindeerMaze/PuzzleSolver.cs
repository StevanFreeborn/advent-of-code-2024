namespace ReindeerMaze;

static class PuzzleSolver
{
  public static int Solve(string[] input, bool isPart2 = false)
  {
    List<Direction> directions = [
      Direction.Right, 
      Direction.Down, 
      Direction.Left,
      Direction.Up,
    ];
    
    var rows = input.Length;
    var columns = input[0].Length;

    var startPosition = new Position(-1, -1);
    var endPosition = new Position(-1, -1);
    
    for (var row = 0; row < rows; row++)
    {
      for (var column = 0; column < columns; column++)
      {
        var currentValue = input[row][column];

        switch (currentValue)
        {
          case 'S':
            startPosition = new(column, row);
            break;
          case 'E':
            endPosition = new(column, row);
            break;
        }
      }
    }

    var queue = new PriorityQueue<MazePath, int>();
    var visitedTiles = new HashSet<Tile>();
    var initialScore = 0;
    var initialDirection = 0;
    var startTile = new Tile(startPosition, initialDirection);
    var bestScore = int.MaxValue;
    var bestPath = new MazePath([startTile], initialScore);
    HashSet<Position> bestSeats = [startPosition];
    
    queue.Enqueue(bestPath, initialScore);
    
    while (queue.Count is not 0)
    {
      var path = queue.Dequeue();
      var currentTile = path.CurrentTile;
      var currentScore = path.Score;
      
      if (currentScore > bestScore)
      {
        break;
      }

      if (currentTile.Position == endPosition)
      {
        bestScore = currentScore;
        bestSeats.UnionWith(path.Tiles.Select(t => t.Position));
        continue;
      }
      
      visitedTiles.Add(currentTile);
      
      var currentDirection = directions[currentTile.DirectionIndex];
      var nextPosition = currentTile.Position.GetNextPosition(currentDirection);

      if (
        nextPosition.X >= 0 && 
        nextPosition.X < columns && 
        nextPosition.Y >= 0 && 
        nextPosition.Y < rows && 
        input[nextPosition.Y][nextPosition.X] != '#'
      )
      {
        var newScoreAfterMovingForward = currentScore + 1;
        var nextTileAfterMovingForward = currentTile with { Position = nextPosition };
        var nextPath = new MazePath([..path.Tiles, nextTileAfterMovingForward], newScoreAfterMovingForward);

        if (visitedTiles.Contains(nextTileAfterMovingForward) is false)
        {
          queue.Enqueue(nextPath, newScoreAfterMovingForward);
        }
      }
 
      var newScoreAfterTurning = currentScore + 1000;
      
      var directionAfterClockWiseTurn = (currentTile.DirectionIndex + 1) % directions.Count;
      var nextTileAfterClockWiseTurn = currentTile with { DirectionIndex = directionAfterClockWiseTurn };

      if (visitedTiles.Contains(nextTileAfterClockWiseTurn) is false)
      {
        queue.Enqueue(new([..path.Tiles, nextTileAfterClockWiseTurn], newScoreAfterTurning), newScoreAfterTurning);
      }
      
      var directionAfterCounterClockWiseTurn = (currentTile.DirectionIndex + 3) % directions.Count;
      var nextTileAfterCounterClockWiseTurn = currentTile with { DirectionIndex = directionAfterCounterClockWiseTurn };

      if (visitedTiles.Contains(nextTileAfterCounterClockWiseTurn) is false)
      {
        queue.Enqueue(new([..path.Tiles, nextTileAfterCounterClockWiseTurn], newScoreAfterTurning), newScoreAfterTurning);
      }
    }
    
    return isPart2 ? bestSeats.Count : bestScore;
  }
}