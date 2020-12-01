using System.Collections.Generic;
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

            var (value1, value2) = FirstAttempt(input);

            return (value1 * value2).ToString();
        }

        private (int value1, int value2) FirstAttempt(ImmutableList<int> input)
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
    }
}
