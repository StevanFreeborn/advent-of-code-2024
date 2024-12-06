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
  
  public bool Validate(Update update)
  {
    var previousPages = new List<int>();
    
    foreach (var page in update.Pages)
    {
      foreach (var previousPage in previousPages)
      {
        if (
          Graph.TryGetValue(previousPage, out HashSet<int>? value) &&
          value.Contains(page) is false
        )
        {
          return false;
        }
      }
      
      previousPages.Add(page);
    }
    
    return true;
  }
}