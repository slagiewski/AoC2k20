using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2k20.Challenges
{
    public class Day9 : IAocChallenge
    {
        public string Evaluate()
        {
            var numbers = File.ReadAllLines("./input/Day9.txt")
                .Select(long.Parse)
                .ToArray();

            return
                $"Part One: {PartOne(numbers)}" + Environment.NewLine +
                $"Part Two: {PartTwo(numbers)}";
        }

        private long PartOne(long[] numbers)
        {
            const int PREAMBULE_LENGTH = 25;

            var previousSums = GetSumsForPreambule(numbers, PREAMBULE_LENGTH);

            for (var i = PREAMBULE_LENGTH; i < numbers.Length; i++)
            {
                var currentNumber = numbers[i];

                if (!previousSums.Contains(currentNumber))
                {
                    return currentNumber;
                }

                for (var j = 0; j < PREAMBULE_LENGTH - 1; j++)
                {
                    previousSums.Dequeue();
                }

                var previousNumbersSlice = numbers.AsSpan(i - (PREAMBULE_LENGTH - 1), PREAMBULE_LENGTH - 1);
                foreach (var previousNumber in previousNumbersSlice)
                {
                    previousSums.Enqueue(previousNumber + currentNumber);
                }
            }

            return -1;
        }
        private long PartTwo(long[] numbers)
        {
            const int FIRST_PART_ANSWER = 373803594;

            for (var setLength = 2; setLength < numbers.Length; setLength++)
            {
                for (var startIndex = 0; startIndex < numbers.Length - (setLength - 1); startIndex++)
                {
                    var contiguousNumberSet = numbers.Skip(startIndex).Take(setLength).ToArray();
                    if (contiguousNumberSet.Sum() == FIRST_PART_ANSWER)
                    {
                        return contiguousNumberSet.Min() + contiguousNumberSet.Max();
                    }
                }
            }

            return -1;
        }

        private Queue<long> GetSumsForPreambule(long[] numbers, long preambuleLength)
        {
            var result = new Queue<long>();

            for (var i = 0; i < preambuleLength - 1; i++)
            {
                for (var j = i + 1; j < preambuleLength + i; j++)
                {
                    result.Enqueue(numbers[i] + numbers[j]);
                }
            }

            return result;
        }

    }
}
