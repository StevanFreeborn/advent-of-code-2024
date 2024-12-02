# Advent of Code 2024

This repository contains my solutions for the [Advent of Code 2024](https://adventofcode.com/2024) challenges.

## Languages

I am using C# for this year's challenge again.

## Structure

Each day's solution is contained in a separate folder. Each folder contains:

- A PROBLEM.md file containing the problem description.
- A INPUT.txt file containing the input for the problem.
- A dotnet console application containing the solution.
- An TUnit test project containing unit tests for the solution.

### Prequesties

- [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

### Running the Solutions

To run the solutions, navigate to the solution folder and run the following command:

```bash
dotnet run -- <path-to-input-file>
```

Or for part 2 solutions

```bash
dotnet run -- <path-to-input-file> part2
```

You can also build the projects and run the executables directly.

```bash
dotnet build
./bin/Debug/net9.0/<project-name> <path-to-input-file>
```

## Challenges

| Day | Problem                    |              Solution              | Notes                 |
|-----|----------------------------|:----------------------------------:|-----------------------|
| 01  | [Problem](./01/PROBLEM.md) | [Solution](./01/HistorianHysteria/) | A great way to start! |
