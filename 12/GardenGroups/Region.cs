namespace GardenGroups;

class Region
{
  public char PlantType { get; }
  public HashSet<Position> Positions { get; } = [];
  public int Area => Positions.Count;
  public int Perimeter { get; private set; }

  private Region(char plantType)
  {
    PlantType = plantType;
  }

  public static Region From(char plantType) => new(plantType);
  public static Region From(Position position) => new(position.PlantType);
  
  public void AddPosition(Position position) => Positions.Add(position);
  public void IncreasePerimeter() => Perimeter++;
  public int CalculateFencePriceBasedOnPerimeter() => Area * Perimeter;
  public int CalculateFencePriceBasedOnSides()
  {
    // corners is always
    // equal to the number
    // of sides
    var corners = FindCorners();
    return corners * Area;
  }

  private int FindCorners()
  {
    var numberOfCorners = 0;

    foreach (var position in Positions)
    {
      var hasNeighborAbove = HasNeighborInDirection(Direction.Up, position);
      var hasNeighborRight = HasNeighborInDirection(Direction.Right, position);
      var hasNeighborBelow = HasNeighborInDirection(Direction.Down, position);
      var hasNeighborLeft = HasNeighborInDirection(Direction.Left, position);
      
      //  X
      // XRX
      //  X
      if (hasNeighborRight is false && hasNeighborAbove is false)
      {
        numberOfCorners++;
      }

      if (hasNeighborRight is false && hasNeighborBelow is false)
      {
        numberOfCorners++;
      }
      
      if (hasNeighborLeft is false && hasNeighborAbove is false)
      {
        numberOfCorners++;
      }

      if (hasNeighborLeft is false && hasNeighborBelow is false)
      {
        numberOfCorners++;
      }
      
      // XRX
      // RRR
      // XRX
      var nextPositionRight = position.GetNextPosition(Direction.Right);
      var nextPositionLeft = position.GetNextPosition(Direction.Left);

      if (hasNeighborRight && hasNeighborAbove && HasNeighborInDirection(Direction.Up, nextPositionRight) is false)
      {
        numberOfCorners++;
      }
      
      if (hasNeighborRight && hasNeighborBelow && HasNeighborInDirection(Direction.Down, nextPositionRight) is false)
      {
        numberOfCorners++;
      }
      
      if (hasNeighborLeft && hasNeighborAbove && HasNeighborInDirection(Direction.Up, nextPositionLeft) is false)
      {
        numberOfCorners++;
      }
      
      if (hasNeighborLeft && hasNeighborBelow && HasNeighborInDirection(Direction.Down, nextPositionLeft) is false)
      {
        numberOfCorners++;
      }
    }
    
    return numberOfCorners;
  }

  private bool HasNeighborInDirection(Direction direction, Position position)
  {
    var nextPosition = position.GetNextPosition(direction);
    return Positions.Any(p => p.X == nextPosition.X && p.Y == nextPosition.Y);
  }
}