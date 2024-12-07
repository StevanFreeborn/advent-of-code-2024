namespace GuardGallivant;

record Direction(char Value)
{
  public static readonly Direction Up = new('^');
  public static readonly Direction Down = new('v');
  public static readonly Direction Left = new('<');
  public static readonly Direction Right = new('>');
}