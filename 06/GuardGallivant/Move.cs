namespace GuardGallivant;

record Move(int XOffset, int YOffset)
{
  public static readonly Move Up = new(0, -1);
  public static readonly Move Down = new(0, 1);
  public static readonly Move Left = new(-1, 0);
  public static readonly Move Right = new(1, 0);
}