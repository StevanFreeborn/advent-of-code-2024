namespace WarehouseWoes;

record Position(int X, int Y)
{
  public int GpsCoordinate => (100 * Y) + X;
  
  public Position GetNextPosition(Direction direction)
  {
    return new(X + direction.Xd, Y + direction.Yd);
  }
}