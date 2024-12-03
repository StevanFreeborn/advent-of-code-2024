namespace RedNosedReports;

class Report(List<int> levels)
{
  public bool IsSafe()
  {
    for (var i = 0; i < levels.Count - 1; i++)
    {
      var current = levels[i];
      var next = levels[i + 1];
      
      if (HasExceededLimits(current, next) || HasChangedDirection(current, next))
      {
        return false;
      }
    }

    return true;
  }

  public bool IsSafeWithProblemDampener()
  {
    for (var currentIndex = 0; currentIndex < levels.Count - 1; currentIndex++)
    {
      var nextIndex = currentIndex + 1;
      var current = levels[currentIndex];
      var next = levels[nextIndex];
      var directionChanged = HasChangedDirection(current, next);

      // problem dampener permits us to try to make
      // levels work after removing one of them.
      // so when we encounter a violation...either direction
      // of levels change or levels change so much that they exceed
      // the limits we are going to attempt to see if
      // the levels will work if we remove one of the levels in
      // the current pair.
      if (HasExceededLimits(current, next) is false && directionChanged is false)
      {
        continue;
      }

      // specific edge case where direction changes
      // on the second pair of levels after the first
      // pair is a valid.
      // i.e. 43, 41, 43, 44, 45, 47, 49
      // in this case we should attempt to make levels
      // work by removing first element so that we can
      // essentially reset what the valid direction is.
      if (directionChanged && currentIndex is 1)
      {
        var levelsWithoutFirstElement = levels.ToList();
        levelsWithoutFirstElement.RemoveAt(0);
        var isSafeWithoutFirstElement = new Report(levelsWithoutFirstElement).IsSafe();

        if (isSafeWithoutFirstElement)
        {
          return true;
        }
      }

      // check if levels work without the current level
      var levelsWithoutCurrent = levels.ToList();
      levelsWithoutCurrent.RemoveAt(currentIndex);
      var isSafeWithoutCurrent = new Report(levelsWithoutCurrent).IsSafe();

      if (isSafeWithoutCurrent)
      {
        return true;
      }

      // check if levels work without the next level
      var levelsWithoutNext = levels.ToList();
      levelsWithoutNext.RemoveAt(nextIndex);
      var isSafeWithoutNext = new Report(levelsWithoutNext).IsSafe();

      // we've exhausted ways that the levels might work
      // so we can just return this check.
      return isSafeWithoutNext;
    }

    return true;
  }

  private static bool HasExceededLimits(int current, int next)
  {
    var delta = Math.Abs(next - current);
    return delta is > 3 or < 1;
  }

  private bool HasChangedDirection(int current, int next)
  {
    var direction = levels[1] > levels[0]
      ? Direction.Increasing
      : Direction.Decreasing;

    var isNextGreaterThanCurrent = next > current;

    switch (direction)
    {
      case Direction.Increasing when isNextGreaterThanCurrent is false:
      case Direction.Decreasing when isNextGreaterThanCurrent:
        return true;
      default:
        return false;
    }
  }
}
