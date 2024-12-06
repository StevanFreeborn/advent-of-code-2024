namespace PrintQueue;

record Update(List<int> Pages)
{
  public int GetMiddlePage()
  {
    return Pages.Count is 0 ? 0 : Pages[Pages.Count / 2];
  }
}