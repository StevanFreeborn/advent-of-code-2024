﻿using System.Diagnostics;

using CeresSearch;

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

var grid = new Grid(input);

var results = isPart2
  ? grid.CountXmases()
  : grid.CountOccurrences("XMAS");
  
stopwatch.Stop();
Console.WriteLine($"The number of occurrences is {results}. ({stopwatch.ElapsedMilliseconds}ms)");