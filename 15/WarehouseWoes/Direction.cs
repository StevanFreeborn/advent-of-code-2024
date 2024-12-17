namespace  WarehouseWoes;

record Direction(int Xd, int Yd)
{
  private const char UpCharacter = '^';
  private const char RightCharacter = '>';
  private const char DownCharacter = 'v';
  private const char LeftCharacter = '<';

  private static readonly Direction Up = new(0, -1);
  private static readonly Direction Down = new(0, 1);
  public static readonly Direction Left = new(-1, 0);
  public static readonly Direction Right = new(1, 0);

  public static Direction From(char character)
  {
    return character switch
    {
      UpCharacter => Up,
      RightCharacter => Right,
      DownCharacter => Down,
      LeftCharacter => Left,
      _ => throw new ArgumentException(@$"{nameof(character)} is not a valid: {character}"),
    };
  }
}