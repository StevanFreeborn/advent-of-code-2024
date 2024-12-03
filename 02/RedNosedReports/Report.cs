namespace RedNosedReports;

class Report(List<int> levels)
{
  private readonly List<int> _levels = levels;

  public string GetDebugOutput()
  {
    return $"{string.Join(' ', _levels)} {IsSafe()} {IsSafeWithProblemDampener()}";
  }

  public bool IsSafe()
  {
    for (var i = 0; i < _levels.Count - 1; i++)
    {
      var current = _levels[i];
      var next = _levels[i + 1];

      if (HasExceededLimits(current, next) || HasChangedDirection(current, next))
      {
        return false;
      }
    }

    return true;
  }

  public bool IsSafeWithProblemDampener()
  {
    for (var currentIndex = 0; currentIndex < _levels.Count - 1; currentIndex++)
    {
      var nextIndex = currentIndex + 1;
      var current = _levels[currentIndex];
      var next = _levels[nextIndex];
      var directionChanged = HasChangedDirection(current, next);

      // problem dampener permits us to try to make
      // levels work after removing one of them.
      // so when we encounter a violation...either direction
      // of levels change or levels change so much that they exceed
      // the limits we are going to attempt to see if
      // the levels will work if we remove one of the levels in
      // the current pair.
      if (HasExceededLimits(current, next) || directionChanged)
      {
        // specific edge case where direction changes
        // on the second pair of levels after the first
        // pair is a valid.
        // i.e. 43, 41, 43, 44, 45, 47, 49
        // in this case we should attempt to make levels
        // work by removing first element so that we can
        // essentially reset what the valid direction is.
        if (directionChanged && currentIndex is 1)
        {
          var levelsWithoutFirstElement = _levels.ToList();
          levelsWithoutFirstElement.RemoveAt(0);
          var isSafeWithoutFirstElement = new Report(levelsWithoutFirstElement).IsSafe();

          if (isSafeWithoutFirstElement)
          {
            return true;
          }
        }

        // check if levels work without the current level
        var levelsWithoutCurrent = _levels.ToList();
        levelsWithoutCurrent.RemoveAt(currentIndex);
        var isSafeWithoutCurrent = new Report(levelsWithoutCurrent).IsSafe();

        if (isSafeWithoutCurrent)
        {
          return true;
        }

        // check if levels work without the next level
        var levelsWithoutNext = _levels.ToList();
        levelsWithoutNext.RemoveAt(nextIndex);
        var isSafeWithoutNext = new Report(levelsWithoutNext).IsSafe();

        if (isSafeWithoutNext)
        {
          return true;
        }

        // if won't work with all levels
        // or without current or next then doesn't
        // work at all.
        return false;
      }
    }

    return true;
  }

  private bool HasExceededLimits(int current, int next)
  {
    var delta = Math.Abs(next - current);

    if (delta > 3 || delta < 1)
    {
      return true;
    }

    return false;
  }

  private bool HasChangedDirection(int current, int next)
  {
    var direction = _levels[1] > _levels[0]
      ? Direction.Increasing
      : Direction.Decreasing;

    var isNextGreaterThanCurrent = next > current;

    if (direction is Direction.Increasing && isNextGreaterThanCurrent is false)
    {
      return true;
    }

    if (direction is Direction.Decreasing && isNextGreaterThanCurrent)
    {
      return true;
    }

    return false;
  }
}
