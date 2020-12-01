using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day1 : IAocChallenge
    {
        public string Evaluate()
        {
            var input = File.ReadAllLines("./input/Day1.txt")
                .Select(int.Parse)
                .ToImmutableList();

            var partOne = PartOne(input);
            var partTwo = PartTwo(input);

            return
                $"Part One: {partOne.value1 * partOne.value2}" + Environment.NewLine +
                $"Part Two: {partTwo.value1 * partTwo.value2 * partTwo.value3}";
        }

        private (int value1, int value2) PartOne(ImmutableList<int> input)
        {
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input.Count; j++)
                {
                    if (i == j) continue;
                    if (input[i] + input[j] == 2020)
                        return (input[i], input[j]);
                }
            }

            return (0, 0);
        }

        private (int value1, int value2, int value3) PartTwo(ImmutableList<int> input)
        {
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input.Count; j++)
                {
                    if (i == j || input[i] + input[j] >= 2020) continue;

                    for (var k = 0; k < input.Count; k++)
                    {
                        if (k == i || k == j) continue;
                        if (input[i] + input[j] + input[k] == 2020)
                            return (input[i], input[j], input[k]);
                    }
                }
            }

            return (0, 0, 0);
        }
    }
}
