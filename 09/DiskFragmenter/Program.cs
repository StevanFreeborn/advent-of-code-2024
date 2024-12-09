using System.Diagnostics;

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
var input = await File.ReadAllTextAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var result = DiskMap.FromDenseFormat(input).CompactDisk().CalculateCheckSum();

stopwatch.Stop();
Console.WriteLine($"The checksum of the compacted disk is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

class DiskMap
{
  private readonly IReadOnlyList<int> _expandedFormat;

  private DiskMap(string denseFormat)
  {
    _expandedFormat = GetExpandedFormat(denseFormat);
  }

  private DiskMap(List<int> expandedFormat)
  {
    _expandedFormat = expandedFormat;
  }

  public long CalculateCheckSum()
  {
    long total = 0;

    foreach (var (fileId, index) in _expandedFormat.Select((block, index) => (block, index)))
    {
      if (fileId is -1)
      {
        continue;
      }
      
      total += fileId * index;
    }

    return total;
  }

  public DiskMap CompactDisk()
  {
    var compactedFormat = _expandedFormat.ToList();
    var blockToBeMovedIndex = compactedFormat.Count - 1;
    
    // TODO: I think this might be easier to deal with in a while loop.
    for (var possiblePlacementIndex = 0; possiblePlacementIndex < compactedFormat.Count; possiblePlacementIndex++)
    {
      if (blockToBeMovedIndex < possiblePlacementIndex)
      {
        break;
      }
      
      var possiblePlacement = compactedFormat[possiblePlacementIndex];
      var blockToBePlaced = compactedFormat[blockToBeMovedIndex];
      
      if (possiblePlacement is not -1)
      {
        continue;
      }
      
      if (blockToBePlaced is -1)
      {
        possiblePlacementIndex--;
        blockToBeMovedIndex--;
        continue;
      }

      compactedFormat[possiblePlacementIndex] = blockToBePlaced;
      compactedFormat[blockToBeMovedIndex] = -1;
      blockToBeMovedIndex--;
    }
    
    return new(compactedFormat);
  }

  public DiskMap CompactDiskWithoutFragmentation()
  {
    var compactedFormat = _expandedFormat.ToList();

    var fileBlockStartIndex = compactedFormat.FindLastIndex(i => i is not -1);
    var fileBlockEndIndex = fileBlockStartIndex;
    
    var freeBlockStartIndex = compactedFormat.IndexOf(-1);
    var freeBlockEndIndex = freeBlockStartIndex;

    while (fileBlockEndIndex > -1)
    {
      while (compactedFormat[fileBlockEndIndex] == compactedFormat[fileBlockStartIndex])
      {
        fileBlockEndIndex--;

        if (fileBlockEndIndex is -1)
        {
          break;
        }
      }
      
      while (freeBlockEndIndex < compactedFormat.Count - 1 && freeBlockEndIndex is not -1)
      {
        while (compactedFormat[freeBlockStartIndex] == compactedFormat[freeBlockEndIndex])
        {
          freeBlockEndIndex++;
        }

        var fileBlockLength = fileBlockStartIndex - fileBlockEndIndex;
        var freeBlockLength = freeBlockEndIndex - freeBlockStartIndex;

        if (freeBlockLength < fileBlockLength || freeBlockStartIndex > file)
        {
          freeBlockStartIndex = compactedFormat.IndexOf(-1, freeBlockEndIndex);
          freeBlockEndIndex = freeBlockStartIndex;
          continue;
        }

        for (int i = freeBlockStartIndex; i < freeBlockEndIndex; i++)
        {
          for (int j = fileBlockStartIndex; j < fileBlockEndIndex; j++)
          {
            var fileIdToMove = compactedFormat[j];
            compactedFormat[i] = fileIdToMove;
            compactedFormat[j] = -1;
          }
        }
      }
      
      fileBlockStartIndex = compactedFormat.FindLastIndex(fileBlockEndIndex, i => i is not -1);
      fileBlockEndIndex = fileBlockStartIndex;
      
      freeBlockStartIndex = compactedFormat.IndexOf(-1);
      freeBlockEndIndex = freeBlockStartIndex;
    }
    
    
    return new(compactedFormat);
  }
  
  public static DiskMap FromDenseFormat(string denseFormat) => new(denseFormat);
  
  private static List<int> GetExpandedFormat(string denseFormat)
  {
    var expandedFormat = new List<int>();
    var fileId = 0;
    
    for (var characterIndex = 0; characterIndex < denseFormat.Length; characterIndex += 2)
    {
      var fileLength = int.Parse(denseFormat[characterIndex].ToString());
      expandedFormat.AddRange(Enumerable.Repeat(fileId, fileLength));

      var freeSpaceIndex = characterIndex + 1;

      if (freeSpaceIndex < denseFormat.Length)
      {
        var freeSpaceLength = int.Parse(denseFormat[characterIndex + 1].ToString());
        expandedFormat.AddRange(Enumerable.Repeat(-1, freeSpaceLength)); 
      }
      
      fileId++;
    }

    return expandedFormat;
  }
}