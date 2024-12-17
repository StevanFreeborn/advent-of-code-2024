namespace WarehouseWoes;

class WideWarehouseMap : Map
{
  private WideWarehouseMap(Dictionary<Position, char> positions) : base(positions) {}
  
  public static WideWarehouseMap From(Dictionary<Position, char> positions) => new(positions);
  public static WideWarehouseMap From(string[] input) => new(ConvertToDictionary(input));
  
  public override Map MoveRobot(List<Direction> directions)
  {
    var positions = Positions.ToDictionary();
    var robotPosition = RobotPosition;

    foreach (var direction in directions)
    {
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
          robotPosition = nextPosition;
          continue;
        case LeftBoxSide:
        case RightBoxSide:
          var newPositions = PushBox(nextPosition, direction, positions);

          if (newPositions == positions)
          {
            break;
          }

          positions = newPositions;
          positions[robotPosition] = Empty;
          positions[nextPosition] = Robot;
          robotPosition = nextPosition;
          break;
      }
    }
    
    return new WideWarehouseMap(positions);
  }

  public override int CalculateTotalBoxGpsCoordinates()
  {
    var boxes = Positions.Where(kvp => kvp.Value is LeftBoxSide).ToList();
    return boxes.Select(kvp => kvp.Key).Sum(p => p.GpsCoordinate);
  }
  
  private static Dictionary<Position, Char> PushBox(Position boxPosition, Direction direction, Dictionary<Position, char> currentPositions)
  {
    var positions = currentPositions.ToDictionary();
    var boxPositions = new Queue<Position>();
    var visitedPositions = new HashSet<Position>();
    boxPositions.Enqueue(boxPosition);
    
    while (boxPositions.Count is not 0)
    {
      var currentPosition = boxPositions.Dequeue();

      if (visitedPositions.Contains(currentPosition))
      {
        continue;
      }

      visitedPositions.Add(currentPosition);
      
      var nextPosition = currentPosition.GetNextPosition(direction);
      var hasCurrentPosition = positions.TryGetValue(currentPosition, out var currentPositionValue);
      var hasNextPosition = positions.TryGetValue(nextPosition, out var nextPositionValue);
      
      if (hasCurrentPosition is false || hasNextPosition is false)
      {
        return currentPositions;
      }

      if (nextPositionValue is Wall)
      {
        return currentPositions;
      }
      
      switch (currentPositionValue)
      {
        case RightBoxSide:
          {
            var leftBoxSidePosition = currentPosition.GetNextPosition(Direction.Left);
            boxPositions.Enqueue(leftBoxSidePosition);
            break;
          }
        case LeftBoxSide:
          {
            var rightBoxSidePosition = currentPosition.GetNextPosition(Direction.Right);
            boxPositions.Enqueue(rightBoxSidePosition);
            break;
          }
      }
      
      if (nextPositionValue is Empty)
      {
        continue;
      }

      boxPositions.Enqueue(nextPosition);
    }

    foreach (var position in visitedPositions.Reverse())
    {
      var nextPosition = position.GetNextPosition(direction);
      
      var currentValue = positions[position];
      var nextValue = positions[nextPosition];

      positions[nextPosition] = currentValue;
      positions[position] = nextValue;
    }
    
    return positions;
  }
}