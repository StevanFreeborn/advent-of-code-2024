namespace ReindeerMaze;

record MazePath(List<Tile> Tiles, int Score)
{
  public Tile CurrentTile => Tiles.Last();
}