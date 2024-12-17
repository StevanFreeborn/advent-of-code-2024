using System.Text;

namespace WarehouseWoes;

abstract class Map(Dictionary<Position, char> positions)
{
  protected const char Robot = '@';
  protected const char Wall = '#';
  protected const char Box = 'O';
  protected const char Empty = '.';
  protected const char LeftBoxSide = '[';
  protected const char RightBoxSide = ']';
  
  public readonly Dictionary<Position, char> Positions = positions;
  protected Position RobotPosition => Positions.First(kvp => kvp.Value is Robot).Key;
  private int MaxX => Positions.Select(kvp => kvp.Key).Max(p => p.X);
  private int MaxY => Positions.Select(kvp => kvp.Key).Max(p => p.Y);
  public string[] MapGrid => ToString().Split(Environment.NewLine);
  
  public abstract int CalculateTotalBoxGpsCoordinates();
  public abstract Map MoveRobot(List<Direction> directions);
  public virtual Map Scale() => this;
  
  protected static Dictionary<Position, char> ConvertToDictionary(string[] input)
  {
    var positions = new Dictionary<Position, char>();
    
    for (var y = 0; y < input.Length; y++)
    {
      for (var x = 0; x < input[0].Length; x++)
      {
        positions.Add(new(x, y), input[y][x]);
      }
    }

    return new(positions);
  }
  
  public override string ToString()
  {
    var map = new StringBuilder();

    for (var y = 0; y <= MaxY; y++)
    {
      var row = new StringBuilder();
      
      for (var x = 0; x <= MaxX; x++)
      {
        var value = Positions[new(x, y)];
        row.Append(value);
      }

      map.AppendLine(row.ToString());
    }

    return map.ToString();
  }
}