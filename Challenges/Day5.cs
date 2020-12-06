using System;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day5 : IAocChallenge
    {
        public string Evaluate()
        {
            var input = File.ReadAllLines("./input/Day5.txt");

            return 
                $"Part One: {PartOne(input)}" + Environment.NewLine +
                $"Part Two: {PartTwo(input)}";
        }


        private int PartOne(string[] input) =>
            input
                .Select(GetSeatId)
                .Max();

        private int PartTwo(string[] input)
        {
            var seatIds = input
                .Select(GetSeatId)
                .ToList();

            var allSeatIds = Enumerable.Range(0, 8)
                .SelectMany(column =>
                    Enumerable.Range(0, 128)
                        .Select(row => GetSeatId(row, column))
                )
                .ToList();

            var missingSeatIds = allSeatIds
                .Except(seatIds)
                .ToList();

            return missingSeatIds.Single(missingSeatId =>
                seatIds.Contains(missingSeatId + 1)
                && seatIds.Contains(missingSeatId - 1)
                );
        }

        private int GetSeatId(string seatCode) => GetSeatId(GetSeatRow(seatCode), GetSeatColumn(seatCode));

        private int GetSeatId(int row, int column) => 8 * row + column;

        private int GetSeatColumn(string seatCode) =>
            Convert.ToInt32(
                seatCode[^3..].Replace('L', '0').Replace('R', '1'),
                2);

        private int GetSeatRow(string seatCode) => 
            Convert.ToInt32(
                seatCode[..7].Replace('F', '0').Replace('B', '1'), 
                2);
    }
}
