namespace ReindeerMaze;

record Position(int X, int Y)
{
  public Position GetNextPosition(Direction direction)
  {
    return new(X + direction.Xd, Y + direction.Yd);
  }
}