namespace GardenGroups;

record Position(int X, int Y, char PlantType)
{
  public Position GetNextPosition(Direction direction) => new(X + direction.Xd, Y + direction.Yd, '.');
  
  public bool TryGetNextPosition(Direction direction, string[] grid, out Position nextPosition)
  {
    nextPosition = GetNextPosition(direction);

    var isOffGrid = IsOffGrid(nextPosition, grid);

    if (isOffGrid)
    {
      return false;
    }
    
    nextPosition = nextPosition with { PlantType = grid[nextPosition.Y][nextPosition.X] };
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