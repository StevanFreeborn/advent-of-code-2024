﻿using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

if (args.Length is 0)
{
  Console.WriteLine("Please provide a path to the input file.");
  return;
}

if (File.Exists(args[0]) is false)
{
  Console.WriteLine("The provided file does not exist.");
  return;
}

var isPart2 = args.Length is 2 && args[1] is "part2";
var input = await File.ReadAllLinesAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var result = Simulation.From(101, 103, input).Run(100);

stopwatch.Stop();
Console.WriteLine($"The safety factor will be {result}. ({stopwatch.ElapsedMilliseconds}ms)");

class Simulation
{
  private readonly List<Robot> _robots;
  private readonly int _width;
  private readonly int _height;
  private readonly List<Quadrant> _quadrants;

  private Simulation(int width, int height, List<Robot> robots)
  {
    _width = width;
    _robots = robots;
    _height = height;

    var middleColumn = _width / 2;
    var middleRow = _height / 2;
    
    var topLeftQuadrant = new Quadrant(0, middleColumn - 1, 0, middleRow - 1);
    var topRightQuadrant = new Quadrant(middleColumn + 1, _width - 1, 0, middleRow - 1);
    var bottomLeftQuadrant = new Quadrant(0, middleColumn - 1, middleRow + 1, _height - 1);
    var bottomRightQuadrant = new Quadrant(middleColumn + 1, _width - 1, middleRow + 1, _height - 1);
    _quadrants = [topLeftQuadrant, topRightQuadrant, bottomLeftQuadrant, bottomRightQuadrant];
  }

  public static Simulation From(int width, int height, string[] input) => new(width, height, input.Select(Robot.From).ToList());

  public int Run(int times)
  {
    for (int i = 0; i < times; i++)
    {
      foreach (var robot in _robots)
      {
        robot.Move(_width, _height);
      }
      
      // Debug(i);
    }

    return _quadrants
      .Select(quadrant => _robots.Count(r => r.IsIn(quadrant)))
      .Aggregate(1, (current, count) => current * count);
  }

  public override string ToString()
  {
    var lines = new StringBuilder();
    
    for (var currentColumn = 0; currentColumn < _width; currentColumn++)
    {
      var row = new StringBuilder();
      
      for (int currentRow = 0; currentRow < _height; currentRow++)
      {
        if (_robots.Any(r => r.IsIn(currentColumn, currentRow)))
        {
          row.Append('R');
        }
        else
        {
          row.Append('.');
        }
      }

      lines.AppendLine(row.ToString());
    }

    return lines.ToString();
  }
}

partial record Robot
{
  private int PositionX { get; set; }
  private int PositionY { get; set; }
  private int VelocityX { get; }
  private int VelocityY { get; }

  private Robot(int positionX, int positionY, int velocityX, int velocityY)
  {
    PositionX = positionX;
    PositionY = positionY;
    VelocityX = velocityX;
    VelocityY = velocityY;
  }

  public static Robot From(
    int positionX, 
    int positionY, 
    int velocityX, 
    int velocityY
  ) => new(positionX, positionY, velocityX, velocityY);
  
  public static Robot From(string input)
  {
    var matches = RobotRegex().Match(input);

    if (matches.Groups.Count is not 5)
    {
      throw new ArgumentException("The given robot input is missing values");
    }
    
    return new(
      int.Parse(matches.Groups[1].Value),
      int.Parse(matches.Groups[2].Value),
      int.Parse(matches.Groups[3].Value),
      int.Parse(matches.Groups[4].Value)
    );
  }

  public (int X, int Y) Move(int MaxX, int MaxY)
  {
    PositionX = (PositionX + VelocityX + MaxX) % MaxX;
    PositionY = (PositionY + VelocityY + MaxY) % MaxY;

    return (PositionX, PositionY);
  }

  public bool IsIn(Quadrant quadrant)
  {
    return PositionX >= quadrant.MinX &&
           PositionX <= quadrant.MaxX &&
           PositionY >= quadrant.MinY &&
           PositionY <= quadrant.MaxY;
  }

  public bool IsIn(int x, int y) => PositionX == x && PositionY == y;

  [GeneratedRegex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
  private static partial Regex RobotRegex();
}

record Quadrant(int MinX, int MaxX, int MinY, int MaxY);