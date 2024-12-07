namespace GuardGallivant;

class Map
{
  private static readonly Dictionary<Direction, Move> MovementDictionary = new()
  {
    { Direction.Up, Move.Up },
    { Direction.Down, Move.Down },
    { Direction.Left, Move.Left },
    { Direction.Right, Move.Right },
  };

  private static readonly Direction[] Directions = MovementDictionary.Keys.ToArray();
  
  private readonly string[] _input;
  
  public Map(string[] input)
  {
    _input = input;
  }
  
  // TODO: This can be optimized I think.
  // Right now I'm basically doing a flood fill...I think
  // I'm finding all the places on the map where I can
  // place the obstruction, placing, and then seeing if
  // a loop is created when guard patrols map.
  // IDEA: Only check places on the guard's path
  public int IdentifyNumberOfPlacementsForNewObstruction()
  {
    var count = 0;
    
    WalkMap((rowIndex, columnIndex) =>
    {
      var currentCharacter = _input[rowIndex][columnIndex];

      if (IsGuard(currentCharacter) || IsBlocked(currentCharacter))
      {
        return;
      }

      var mapWithNewObstruction = _input.ToArray();
      var row = mapWithNewObstruction[rowIndex].ToCharArray();
      row[columnIndex] = '#';
      mapWithNewObstruction[rowIndex] = new string(row);

      var hasLoop = new Map(mapWithNewObstruction).DetectLoop();

      if (hasLoop)
      {
        count++;
      }
    });

    return count;
  }

  private bool DetectLoop()
  {
    var originalGuardPosition = GetGuardPosition();
    var currentGuardPosition = originalGuardPosition;

    var blockedPositions = new List<GuardPosition>();
    
    while (true)
    {
      var nextMove = MovementDictionary[currentGuardPosition.Direction];
      
      var nextGuardPosition = currentGuardPosition with
      {
        RowIndex = currentGuardPosition.RowIndex + nextMove.YOffset,
        ColumnIndex = currentGuardPosition.ColumnIndex + nextMove.XOffset,
      };
      
      if (IsOffGrid(nextGuardPosition))
      {
        return false;
      }

      if (IsBlocked(nextGuardPosition))
      {
        if (blockedPositions.Contains(nextGuardPosition))
        {
          return true;
        }
        
        blockedPositions.Add(nextGuardPosition);
        
        var newDirection = TurnRight(currentGuardPosition);
        currentGuardPosition = currentGuardPosition with { Direction = newDirection };
        
        continue;
      }

      currentGuardPosition = nextGuardPosition;
    }
  }
  
  public int PredictDistinctPositionsCount()
  {
    var currentGuardPosition = GetGuardPosition();
    var uniquePositions = new HashSet<(int x, int y)>()
    {
      (currentGuardPosition.ColumnIndex, currentGuardPosition.RowIndex),
    };
    
    while (true)
    {
      var nextMove = MovementDictionary[currentGuardPosition.Direction];
      
      var nextGuardPosition = currentGuardPosition with
      {
        RowIndex = currentGuardPosition.RowIndex + nextMove.YOffset,
        ColumnIndex = currentGuardPosition.ColumnIndex + nextMove.XOffset,
      };
      
      if (IsOffGrid(nextGuardPosition))
      {
        break;
      }

      if (IsBlocked(nextGuardPosition))
      {
        var newDirection = TurnRight(currentGuardPosition);
        currentGuardPosition = currentGuardPosition with { Direction = newDirection };
        continue;
      }
      
      currentGuardPosition = nextGuardPosition;
      uniquePositions.Add((currentGuardPosition.ColumnIndex, currentGuardPosition.RowIndex));
    }

    return uniquePositions.Count;
  }
  
  private GuardPosition GetGuardPosition()
  {
    var guardPosition = new GuardPosition(-1, -1, new Direction('X'));
    
    WalkMap((rowIndex, columnIndex) =>
    {
      var possibleGuard = new Direction(_input[rowIndex][columnIndex]);
      
      if (Directions.Contains(possibleGuard) is false)
      {
        return;
      }
      
      guardPosition = new GuardPosition(rowIndex, columnIndex, possibleGuard);
    });
    
    return guardPosition;
  }
  
  private void WalkMap(Action<int, int> callback)
  {
    for (var rowIndex = 0; rowIndex < _input.Length; rowIndex++)
    {
      var row = _input[rowIndex];

      for (var columnIndex = 0; columnIndex < row.Length; columnIndex++)
      {
        callback(rowIndex, columnIndex);
      }
    }
  }
  
  private bool IsOffGrid(GuardPosition position)
  {
    return position.RowIndex < 0 ||
           position.RowIndex > _input.Length - 1 ||
           position.ColumnIndex < 0 ||
           position.ColumnIndex > _input[0].Length - 1;
  }

  private bool IsBlocked(GuardPosition position)
  {
    return _input[position.RowIndex][position.ColumnIndex] is '#';
  }

  private bool IsBlocked(char character)
  {
    return character is '#';
  }

  private bool IsGuard(char currentCharacter)
  {
    return Directions.Contains(new Direction(currentCharacter));
  }

  private static Direction TurnRight(GuardPosition guardPosition)
  {
    if (guardPosition.Direction == Direction.Up)
    {
      return Direction.Right;
    }

    if (guardPosition.Direction == Direction.Right)
    {
      return Direction.Down;
    }

    if (guardPosition.Direction == Direction.Down)
    {
      return Direction.Left;
    }

    return Direction.Up;
  }
}
