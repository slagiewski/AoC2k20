using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day3 : IAocChallenge
    {
        public string Evaluate()
        {
            var input = File.ReadAllLines("./input/Day3.txt")
                .Select(l => l.ToCharArray())
                .ToImmutableList();

            return 
                $"Part One: {PartOne(input)}" + Environment.NewLine +
                $"Part Two: {PartTwo(input)}";
        }

        private long PartTwo(ImmutableList<char[]> input) =>
            new []
            {
                (right: 1, down: 1),
                (right: 3, down: 1),
                (right: 5, down: 1),
                (right: 7, down: 1),
                (right: 1, down: 2)
            }
            .Aggregate(1L, (acc, x) => acc * GetNumOfTrees(input, x.right, x.down));

        private int PartOne(ImmutableList<char[]> input) => GetNumOfTrees(input, 3, 1);

        private int GetNumOfTrees(ImmutableList<char[]> input, int rightStepSize, int downStepSize)
        {
            var currentX = rightStepSize;
            var currentY = downStepSize;

            var slopeWidth = input[0].Length;
            var slopeLength = input.Count;

            var numOfTrees = 0;

            while (currentY < slopeLength)
            {
                if (input[currentY][currentX] == '#')
                {
                    numOfTrees++;
                }

                currentY += downStepSize;
                currentX = (currentX + rightStepSize) % slopeWidth;
            }

            return numOfTrees;
        }
    }
}
