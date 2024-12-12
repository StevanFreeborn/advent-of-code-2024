namespace PlutonianPebbles;

class Observer
{
  private readonly List<Stone> _stones;

  private Observer(List<Stone> stones)
  {
    _stones = stones;
  }

  public static Observer Of(List<Stone> stones) => new(stones);
  
  public long Blink(int times)
  {
    var stoneCounts = _stones.GroupBy(s => s)
      .ToDictionary(g => g.Key, g => (long)g.Count());
    
    for (var blinks = 0; blinks < times; blinks++)
    {
      var nextCounts = new Dictionary<Stone, long>();
      
      foreach (var (stone, count) in stoneCounts)
      {
        var transformed = stone.Transform();

        foreach (var result in transformed)
        {
          if (nextCounts.ContainsKey(result))
          {
            nextCounts[result] += count;
          }
          else
          {
            nextCounts[result] = count;
          }
        }
      }

      stoneCounts = nextCounts;
    }
    
    return stoneCounts.Values.Sum();
  }
}