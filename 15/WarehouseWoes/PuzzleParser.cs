namespace WarehouseWoes;

static class PuzzleParser
{
  public static (Map Map, List<Direction> Directions) Parse(string input)
  {
    var inputParts = input.Split($"{Environment.NewLine}{Environment.NewLine}");
    
    var map = WarehouseMap.From(inputParts[0].Split(Environment.NewLine));
    
    var directions = inputParts[1]
      .Where(character =>
      {
        var str = character.ToString();
        return str != "\r" && str != "\n" && str != "\r\n";
      })
      .Select(Direction.From)
      .ToList();
    
    return (map, directions);
  }
}