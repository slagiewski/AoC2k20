using System;
using System.Collections.Generic;
using AoC2k20.Challenges;

namespace AoC2k20
{
    class Program
    {
        static void Main(string[] args)
        {
            var challenges = new Dictionary<string, Func<string>>()
            {
                { "Day 1", new Challenges.Day1().Evaluate },
                { "Day 2", new Challenges.Day2().Evaluate },
                { "Day 3", new Challenges.Day3().Evaluate },
                { "Day 4", new Challenges.Day4().Evaluate },
                { "Day 5", new Challenges.Day5().Evaluate },
                { "Day 6", new Challenges.Day6().Evaluate },
                { "Day 7", new Challenges.Day7().Evaluate },
                { "Day 8", new Challenges.Day8().Evaluate },
                { "Day 9", new Challenges.Day9().Evaluate },
                { "Day 10", new Challenges.Day10().Evaluate },
                { "Day 11", new Challenges.Day11().Evaluate },
            };

            foreach (var (challengeName, evaluate) in challenges)
            {
                Console.WriteLine($"==== {challengeName} ====");
                Console.WriteLine(evaluate());
                Console.WriteLine(Environment.NewLine);
            }

            Console.ReadKey();
        }
    }
}