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

var map = DiskMap.FromDenseFormat(input);
var compactedMap = isPart2
  ? map.CompactDiskWithoutFragmentation()
  : map.CompactDisk();

var result = compactedMap.CalculateCheckSum();

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

  // TODO: Can this be solved without less index manipulation?
  // Can I create a data structure that suits both ways
  // of doing disk compaction and allows for easily calculating
  // the checksum
  // maybe something like [block]
  // block needs to represent a block of free space
  // and a block of file ids
  // each will consist indexes, value
  public DiskMap CompactDisk()
  {
    var compactedFormat = _expandedFormat.ToList();
    var blockToBeMovedIndex = compactedFormat.Count - 1;
    
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
    
    // initialize file block pointers
    var fileBlockStartIndex = compactedFormat.FindLastIndex(i => i is not -1);
    var fileBlockEndIndex = fileBlockStartIndex;
    
    // initialize free block points
    var freeBlockStartIndex = compactedFormat.IndexOf(-1);
    var freeBlockEndIndex = freeBlockStartIndex;
    
    // loop over all possible file blocks
    while (fileBlockEndIndex > -1)
    {
      // loop to end of current file block
      while (compactedFormat[fileBlockEndIndex] == compactedFormat[fileBlockStartIndex])
      {
        fileBlockEndIndex--;

        if (fileBlockEndIndex is -1)
        {
          break;
        }
      }
      
      // loop over all free blocks that are to the left of the
      // current file block
      while (
        freeBlockEndIndex < compactedFormat.Count - 1 && 
        freeBlockEndIndex is not -1 &&
        freeBlockEndIndex < fileBlockEndIndex
      )
      {
        // find end of current free block
        while (compactedFormat[freeBlockStartIndex] == compactedFormat[freeBlockEndIndex])
        {
          freeBlockEndIndex++;
        }

        // determine if free block can hold file block
        var fileBlockLength = fileBlockStartIndex - fileBlockEndIndex;
        var freeBlockLength = freeBlockEndIndex - freeBlockStartIndex;

        if (freeBlockLength < fileBlockLength)
        {
          // if current free block cannot hold file block
          // advance to next free block
          freeBlockStartIndex = compactedFormat.IndexOf(-1, freeBlockEndIndex);
          freeBlockEndIndex = freeBlockStartIndex;
          continue;
        }
        
        // if current free block can hold file block
        // loop over the free block positions and place
        // the file block's id in them, but only replace
        // as many free blocks as there are file blocks
        var blockLengthDelta = freeBlockLength - fileBlockLength;

        for (var i = freeBlockStartIndex; i < freeBlockEndIndex - blockLengthDelta; i++)
        {
          compactedFormat[i] = compactedFormat[fileBlockStartIndex];
        }
        
        // loop over original file block positions
        // and free them up.
        for (var j = fileBlockStartIndex; j > fileBlockEndIndex; j--)
        {
          compactedFormat[j] = -1;
        }

        break;
      }
      
      // if we have reached the end of all
      // possible file blocks then no
      // need to go to next file block
      if (fileBlockEndIndex is -1)
      {
        break;
      }
      
      // move to next file block
      fileBlockStartIndex = compactedFormat.FindLastIndex(fileBlockEndIndex, i => i is not -1);
      fileBlockEndIndex = fileBlockStartIndex;
      
      // reset free block to beginning free block
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