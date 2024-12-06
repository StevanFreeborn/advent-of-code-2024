namespace PrintQueue;

class UpdateValidator
{
  public readonly Dictionary<int, HashSet<int>> Graph = [];

  public UpdateValidator(List<OrderRule> rules)
  {
    foreach (var rule in rules)
    {
      if (Graph.ContainsKey(rule.X) is false)
      {
        Graph.Add(rule.X, []);
      }

      if (Graph.ContainsKey(rule.Y) is false)
      {
        Graph.Add(rule.Y, []);
      }
      
      Graph[rule.X].Add(rule.Y);
    }
  }

  public Update Sort(Update update)
  {
    var sortedUpdate = new Update(update.Pages.ToList());

    while (Validate(sortedUpdate) is false)
    {
      for (var i = 0; i < sortedUpdate.Pages.Count - 1; i++)
      {
        var currentPage = sortedUpdate.Pages[i];
        var nextPage = sortedUpdate.Pages[i + 1];

        if (IsOutOfOrder(currentPage, nextPage) is false)
        {
          continue;
        }

        sortedUpdate.Pages[i] = nextPage;
        sortedUpdate.Pages[i + 1] = currentPage;
      }
    }

    return sortedUpdate;
  }
  
  public bool Validate(Update update)
  {
    var previousPages = new List<int>();
    
    foreach (var page in update.Pages)
    {
      if (previousPages.Any(previousPage => IsOutOfOrder(previousPage, page)))
      {
        return false;
      }

      previousPages.Add(page);
    }
    
    return true;
  }

  private bool IsOutOfOrder(int previousPage, int futurePage)
  {
    return Graph.TryGetValue(previousPage, out var dependents) &&
           dependents.Contains(futurePage) is false;
  }
}