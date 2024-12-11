namespace HoofIt;

record Position(int X, int Y, int Height)
{
  public bool TryGetNextPosition(Direction direction, string[] grid, out Position nextPosition)
  {
    var x = X + direction.XD;
    var y = Y + direction.YD;

    nextPosition = new(x, y, -1);

    var isOffGrid = IsOffGrid(nextPosition, grid);

    if (isOffGrid)
    {
      return false;
    }

    if (int.TryParse(grid[y][x].ToString(), out var nextPositionHeight) is false)
    {
      return false;
    }
    
    nextPosition = nextPosition with { Height = nextPositionHeight };
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