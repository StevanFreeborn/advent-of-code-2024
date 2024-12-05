namespace CeresSearch;

record Direction(int XOffset, int YOffset)
{
  public static readonly Direction Up = new(0, -1);
  public static readonly Direction Down = new(0, 1);
  public static readonly Direction Left = new(-1, 0);
  public static readonly Direction Right = new(1, 0);
  public static readonly Direction DownToTheRight = new(1, 1);
  public static readonly Direction DownToTheLeft = new(-1, 1);
  public static readonly Direction UpToTheRight = new(1, -1);
  public static readonly Direction UpToTheLeft = new(-1, -1);
}

class Grid(string[] input)
{
  private readonly Direction[] _directions = [
    Direction.UpToTheLeft,
    Direction.UpToTheRight,
    Direction.DownToTheLeft,
    Direction.DownToTheRight,
    Direction.Up,
    Direction.Down,
    Direction.Left,
    Direction.Right,
  ];

  public int CountXmases()
  {
    var total = 0;
    
    WalkGrid((rowIndex, columnIndex) =>
    {
      if (FoundXmas(rowIndex, columnIndex))
      {
        total++;
      }
    });

    return total;
  }
  
  public int CountOccurrences(string word)
  {
    var total = 0;

    WalkGrid((rowIndex, columnIndex) => 
      total += _directions.Count(direction => FoundWord(word, direction, rowIndex, columnIndex))
    );

    return total;
  }

  private void WalkGrid(Action<int, int> callback)
  {
    for (var rowIndex = 0; rowIndex < input.Length; rowIndex++)
    {
      var row = input[rowIndex];
      
      for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
      {
        callback(rowIndex, columnIndex);
      }
    }
  }

  private bool FoundXmas(int rowIndex, int colIndex)
  {
    var currentCharacter = input[rowIndex][colIndex];

    if (currentCharacter is not 'A')
    {
      return false;
    }

    var xmasMap = new Dictionary<Direction, char>();

    foreach (var direction in _directions)
    {
      if (
        direction == Direction.Up || 
        direction == Direction.Down || 
        direction == Direction.Left ||
        direction == Direction.Right
      )
      {
        continue;
      }

      var rowIndexToCheck = rowIndex + direction.XOffset;
      var colIndexToCheck = colIndex + direction.YOffset;

      if (IsOffGrid(rowIndexToCheck, colIndexToCheck))
      {
        return false;
      }

      var characterToCheck = input[rowIndexToCheck][colIndexToCheck];

      if (characterToCheck != 'S' && characterToCheck != 'M')
      {
        return false;
      }
      
      xmasMap.Add(direction, characterToCheck);
    }

    return IsValidXmas(xmasMap);
  }

  private static bool IsValidXmas(Dictionary<Direction, char> map)
  {
    var upToTheRight = map[Direction.UpToTheRight];
    var downToTheLeft = map[Direction.DownToTheLeft];
    var upToTheLeft = map[Direction.UpToTheLeft];
    var downToTheRight = map[Direction.DownToTheRight];
    
    return upToTheRight != downToTheLeft && upToTheLeft != downToTheRight;
  }

  private bool FoundWord(string word, Direction direction, int rowIndex, int columnIndex)
  {
    for (var currentTargetCharacterIndex = 0; currentTargetCharacterIndex < word.Length; currentTargetCharacterIndex++)
    {
      var currentTargetCharacter = word[currentTargetCharacterIndex];
      var currentCharacterRowIndex = rowIndex + (direction.XOffset * currentTargetCharacterIndex);
      var currentCharacterColumnIndex = columnIndex + (direction.YOffset * currentTargetCharacterIndex);
      
      if (IsOffGrid(currentCharacterRowIndex, currentCharacterColumnIndex))
      {
        return false;
      }
      
      var currentCharacter = input[currentCharacterRowIndex][currentCharacterColumnIndex];

      if (currentCharacter != currentTargetCharacter)
      {
        return false;
      }
    }
    
    return true;
  }

  private bool IsOffGrid(int rowIndex, int columnIndex)
  {
    return rowIndex < 0 ||
           rowIndex > input.Length - 1 ||
           columnIndex < 0 ||
           columnIndex > input[0].Length - 1;
  }
}