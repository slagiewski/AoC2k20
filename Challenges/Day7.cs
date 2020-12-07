using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day7 : IAocChallenge
    {
        public string Evaluate()
        {
            var bags = File.ReadAllLines("./input/Day7.txt")
                .Select(Bag.FromBagString)
                .ToImmutableList();

            return
                $"Part One: {PartOne(bags)}" + Environment.NewLine +
                $"Part Two: {PartTwo(bags)}";
        }

        private int PartOne(ImmutableList<Bag> bagRules)
        {
            var rules = bagRules.ToDictionary(b => b.Color, b => b.ContainedBags);
            var validBags = FindThatWillContain(rules, "shiny gold");
            return validBags.Count;
        }

        private long PartTwo(ImmutableList<Bag> bagRules)
        {
            var result = FindContainingBagsCount(bagRules, "shiny gold");
            return result;
        }
        
        private long FindContainingBagsCount(ImmutableList<Bag> bagRules, string targetColor)
        {
            var containingBag = bagRules
                .First(bagRule => bagRule.Color == targetColor);
            
            if (!containingBag.ContainedBags.Any())
                return 1;

            return containingBag.ContainedBags
                .Sum(containedBag =>
                    bagRules.First(bagRule => bagRule.Color == containedBag.Key).ContainedBags.Any()
                        ? containedBag.Value + containedBag.Value * FindContainingBagsCount(bagRules, containedBag.Key)
                        : containedBag.Value * FindContainingBagsCount(bagRules, containedBag.Key));
        }

        private List<string> FindThatWillContain(Dictionary<string, Dictionary<string, int>> bagRules, string colorToContain)
        {
            return FindThatWillContain(bagRules, new List<string>() { colorToContain }, new HashSet<string>(), new HashSet<string>());
        }

        private List<string> FindThatWillContain(Dictionary<string, Dictionary<string, int>> bagRules, List<string> colorsToContain, HashSet<string> visited, HashSet<string> result)
        {
            if (visited.Count == bagRules.Count) return result.ToList();

            var containingBags = bagRules
                .Where(kv => colorsToContain.Any(kv.Value.ContainsKey))
                .Select(kv => kv.Key)
                .ToImmutableList();

            if (!containingBags.Any()) return result.ToList();

            visited.UnionWith(colorsToContain);
            result.UnionWith(containingBags);

            return FindThatWillContain(bagRules, containingBags.Except(visited).ToList(), visited, result);
        }

        public class Bag
        {
            public string Color { get; }
            public Dictionary<string, int> ContainedBags { get; }

            private Bag(string color, Dictionary<string, int> containedBagsWithQuantity)
            {
                Color = color;
                ContainedBags = containedBagsWithQuantity;
            }

            public override bool Equals(object? obj)
            {
                return obj is Bag bag && bag.Color == Color;
            }

            public override int GetHashCode()
            {
                return Color.GetHashCode();
            }

            public static Bag FromBagString(string bagRuleStr)
            {
                var bagAndRule = bagRuleStr
                    .TrimEnd('.')
                    .Replace(" bags", string.Empty)
                    .Replace(" bag", string.Empty)
                    .Split(" contain ");
                var color = bagAndRule[0];
                var bags = bagAndRule[1].Contains("no other")
                    ? new Dictionary<string, int>()
                    : bagAndRule[1]
                        .Split(", ")
                        .ToDictionary(
                            bagRule => bagRule[2..], 
                            bagRule => (int)char.GetNumericValue(bagRule[0]));
                return new Bag(color, bags);
            }
        }
    }
}
