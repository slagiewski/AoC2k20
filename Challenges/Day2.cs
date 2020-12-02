using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day2 : IAocChallenge
    {
        public string Evaluate()
        {
            var input = File.ReadAllLines("./input/Day2.txt").ToImmutableList();

            return
                $"Part One: {PartOne(input)}" + Environment.NewLine +
                $"Part Two: {PartTwo(input)}";
        }

        private static string PartOne(ImmutableList<string> input) =>
            input
                .Select(str =>
                {
                    var policyAndPassword = str.Split(": ");
                    var minMaxPolicy = MinMaxPolicy.FromPolicyString(policyAndPassword[0]);
                    return new Password(minMaxPolicy, policyAndPassword[1]);
                })
                .Count(p => p.IsValid)
                .ToString();

        private static string PartTwo(ImmutableList<string> input) =>
            input
                .Select(str =>
                {
                    var policyAndPassword = str.Split(": ");
                    var containsPolicy = ContainsOnePolicy.FromPolicyString(policyAndPassword[0]);
                    return new Password(containsPolicy, policyAndPassword[1]);
                })
                .Count(p => p.IsValid)
                .ToString();
    }

    public class Password
    {
        public IPolicy Policy { get; }
        public string PasswordText { get; }

        public bool IsValid => Policy.Verify(PasswordText);

        public Password(IPolicy policy, string password)
        {
            Policy = policy;
            PasswordText = password;
        }
    }

    public class ContainsOnePolicy : IPolicy
    {
        public int FirstPosition { get; }
        public int SecondPosition { get; }
        public char Char { get; }

        private ContainsOnePolicy(int existsPosition, int notExistsPosition, char character)
        {
            Char = character;
            FirstPosition = existsPosition;
            SecondPosition = notExistsPosition;
        }

        public static ContainsOnePolicy FromPolicyString(string policyStr)
        {   //str in template: '1-3 a'
            var separated = policyStr.Split(" ");
            var positions = separated[0].Split("-");
            return new ContainsOnePolicy(
                int.Parse(positions[0]),
                int.Parse(positions[1]),
                separated[1].First());
        }

        public bool Verify(string passwordText) =>
            (passwordText.ElementAtOrDefault(FirstPosition - 1) == Char) ^
            (passwordText.ElementAtOrDefault(SecondPosition - 1) == Char);
    }

    public class MinMaxPolicy : IPolicy
    {
        public int MinCount { get; }
        public int MaxCount { get; }
        public char Char { get; }

        private MinMaxPolicy(int min, int max, char character)
        {
            Char = character;
            MinCount = min;
            MaxCount = max;
        }

        public static MinMaxPolicy FromPolicyString(string policyStr)
        {   //str in template: '1-3 a'
            var separated = policyStr.Split(" ");
            var ranges = separated[0].Split("-");
            return new MinMaxPolicy(int.Parse(ranges[0]), int.Parse(ranges[1]), separated[1].First());
        }

        public bool Verify(string passwordText)
        {
            var count = passwordText.Count(c => c == Char);
            return count >= MinCount && count <= MaxCount;
        }
    }

    public interface IPolicy
    {
        bool Verify(string passwordText);
    }
}
