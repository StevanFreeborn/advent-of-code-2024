namespace ReindeerMaze;

record Direction(int Xd, int Yd)
{
  public static readonly Direction Up = new(0, -1);
  public static readonly Direction Down = new(0, 1);
  public static readonly Direction Left = new(-1, 0);
  public static readonly Direction Right = new(1, 0);
}