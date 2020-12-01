using System;
using System.Collections.Generic;

namespace AoC2k20
{
    class Program
    {
        static void Main(string[] args)
        {
            var challenges = new Dictionary<string, Func<string>>()
            {
                { "Day1", new Challenges.Day1().Evaluate }
            };

            foreach (var (challengeName, evaluate) in challenges)
            {
                Console.WriteLine($"==== {challengeName} ====");
                Console.WriteLine(evaluate());
            }

            Console.ReadKey();
            
        }
    }
}
