class PuzzleSolver
{
  public int CalculateTotalDistance(List<int> left, List<int> right)
  {
    var leftSorted = left.OrderBy(i => i).ToList();
    var rightSorted = right.OrderBy(i => i).ToList();
    var total = 0;

    foreach (var (leftNumber, index) in leftSorted.Select((num, index) => (num, index)))
    {
      var rightNumber = rightSorted[index];
      total += Math.Abs(leftNumber - rightNumber);
    }

    return total;
  }

  public int CalculateSimilarityScore(List<int> left, List<int> right)
  {
    var total = 0;

    foreach (var num in left)
    {
      var count = right.Count(i => i == num);
      total += num * count;
    }

    return total;
  }
}