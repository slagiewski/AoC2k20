using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day10 : IAocChallenge  
    {
        public string Evaluate()
        {
            var input = File.ReadAllLines("./input/Day10.txt")
                .Select(int.Parse)
                .ToImmutableArray();

            return
                $"Part One: {PartOne(input)}" + Environment.NewLine +
                $"Part Two: {PartTwoFaster(input)}";
        }

        private int PartOne(ImmutableArray<int> input)
        {
            var sortedInput = InitJoltagesArray(input);

            Span<int> differenceCounts = stackalloc int[3];

            for (var i = 1; i < sortedInput.Count; i++)
            {
                var diff = sortedInput[i] - sortedInput[i - 1];
                differenceCounts[diff - 1]++;
            }

            return differenceCounts[0] * differenceCounts[2];
        }

        private long PartTwoSlow(ImmutableArray<int> input)
        {
            var sortedInput = InitJoltagesArray(input);

            long PartTwo(List<int> joltages, int currentIndex)
            {
                if (currentIndex == joltages.Count - 1) return 1;

                var nextPossibleJoltageIndexes = joltages
                    .Skip(currentIndex + 1)
                    .Take(3)
                    .Select((possibleJoltage , i) => (possibleJoltage: possibleJoltage, index: currentIndex + 1 + i))
                    .Where(possibleJoltageWithIndex => possibleJoltageWithIndex.possibleJoltage - joltages[currentIndex] <= 3)
                    .Select(possibleJoltageWithIndex => possibleJoltageWithIndex.index);

                var sum = 0L;
                foreach (var nextPossibleJoltage in nextPossibleJoltageIndexes)
                {
                    sum += PartTwo(joltages, nextPossibleJoltage);
                }

                return sum;
            }

            return PartTwo(sortedInput, 0);
        }

        private long PartTwoFaster(ImmutableArray<int> input)
        {
            var sortedInput = InitJoltagesArray(input);

            var steps = new long[sortedInput.Count];
            steps[0] = 1;

            for (var i = 1; i < sortedInput.Count; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    if (sortedInput[i] - sortedInput[j] <= 3)
                    {
                        steps[i] += steps[j];
                    }
                }
            }

            return steps.Last();
        }

        private static List<int> InitJoltagesArray(ImmutableArray<int> input)
        {
            void AddSortedAdapterJoltages(List<int> joltages) => joltages.AddRange(input.OrderBy(i => i));
            void AddWallSocketJoltage(List<int> joltages) => joltages.Add(0);
            void AddEndDeviceJoltage(List<int> joltages) => joltages.Add(joltages.Last() + 3);

            var sortedInput = new List<int>(input.Length + 2);

            AddWallSocketJoltage(sortedInput);
            AddSortedAdapterJoltages(sortedInput);
            AddEndDeviceJoltage(sortedInput);

            return sortedInput;
        }
    }
}