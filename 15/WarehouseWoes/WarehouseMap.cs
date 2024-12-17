namespace WarehouseWoes;

class WarehouseMap : Map
{
  private WarehouseMap(Dictionary<Position, char> positions) : base(positions) {}

  public static WarehouseMap From(string[] input) => new(ConvertToDictionary(input));

  public override Map Scale()
  {
    var scaledPositions = new Dictionary<Position, char>();
    var scaleFactor = 2;
    
    foreach (var (position, value) in Positions)
    {
      var scaledX = position.X * scaleFactor;
      var scaledY = position.Y;

      switch (value)
      {
        case Wall:
          scaledPositions[new(scaledX, scaledY)] = Wall;
          scaledPositions[new(scaledX + 1, scaledY)] = Wall;
          break;
        case Box:
          scaledPositions[new(scaledX, scaledY)] = WideWarehouseMap.LeftBoxSide;
          scaledPositions[new(scaledX + 1, scaledY)] = WideWarehouseMap.RightBoxSide;
          break;
        case Empty:
          scaledPositions[new(scaledX, scaledY)] = Empty;
          scaledPositions[new(scaledX + 1, scaledY)] = Empty;
          break;
        case Robot:
          scaledPositions[new(scaledX, scaledY)] = Robot;
          scaledPositions[new(scaledX + 1, scaledY)] = Empty;
          break;
        default:
          throw new ApplicationException($"Unknown value present {value}");
      }
    }
    
    return WideWarehouseMap.From(scaledPositions);
  }

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
        case Box when CanPushBox(nextPosition, direction, positions):
          positions = PushBox(nextPosition, direction, positions);
          positions[robotPosition] = Empty;
          positions[nextPosition] = Robot;
          robotPosition = nextPosition;
          break;
      }
    }
    
    return new WarehouseMap(positions);
  }

  public override int CalculateTotalBoxGpsCoordinates()
  {
    var boxes = Positions.Where(kvp => kvp.Value is Box).ToList();
    return boxes.Select(kvp => kvp.Key).Sum(p => p.GpsCoordinate);
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