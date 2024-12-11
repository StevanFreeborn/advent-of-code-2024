namespace HoofIt;

record Direction(int XD, int YD)
{
  public static Direction Up = new(0, -1);
  public static Direction Down = new(0, 1);
  public static Direction Left = new(-1, 0);
  public static Direction Right = new(1, 0);
}