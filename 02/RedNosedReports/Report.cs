namespace RedNosedReports;

class Report(List<int> levels)
{
  private readonly List<int> _levels = levels;

  public bool IsSafe()
  {
    for (var i = 0; i < _levels.Count - 1; i++)
    {
      if (CheckIfSafe(_levels[i], _levels[i + 1]) is false)
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

      if (CheckIfSafe(current, next) is false)
      {
        var levelsWithoutCurrent = _levels.ToList();
        levelsWithoutCurrent.RemoveAt(currentIndex);
        var isSafeWithoutCurrent = new Report(levelsWithoutCurrent).IsSafe();

        if (isSafeWithoutCurrent)
        {
          return true;
        }

        var levelsWithoutNext = _levels.ToList();
        levelsWithoutNext.RemoveAt(nextIndex);
        var isSafeWithoutNext = new Report(levelsWithoutNext).IsSafe();

        if (isSafeWithoutNext)
        {
          return true;
        }

        return false;
      }
    }

    return true;
  }

  private bool CheckIfSafe(int current, int next)
  {
    var direction = _levels[1] > _levels[0]
      ? Direction.Increasing
      : Direction.Decreasing;

    var isNextGreaterThanCurrent = next > current;
    var delta = Math.Abs(next - current);

    if (delta > 3 || delta < 1)
    {
      return false;
    }

    if (direction is Direction.Increasing && isNextGreaterThanCurrent is false)
    {
      return false;
    }

    if (direction is Direction.Decreasing && isNextGreaterThanCurrent)
    {
      return false;
    }

    return true;
  }
}
