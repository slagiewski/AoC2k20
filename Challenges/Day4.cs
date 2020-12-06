using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2k20.Challenges
{
    public class Day4 : IAocChallenge
    {
        public string Evaluate()
        {
            var documentSeparator = "\n\n";

            var input = File.ReadAllText("./input/Day4.txt")
                .TrimEnd()
                .Split(documentSeparator)
                .Select(documentStr =>
                    documentStr
                        .Split('\n', ' ')
                        .Select(fieldStr => fieldStr.Split(':'))
                        .ToImmutableDictionary(kvArr => kvArr[0], kvArr => kvArr[1])
                )
                .ToImmutableList();

            return
                $"Part One: {PartOne(input)}" + Environment.NewLine +
                $"Part Two: {PartTwo(input)}";
        }

        private int PartOne(ImmutableList<ImmutableDictionary<string, string>> input) =>
            input.Count(ContainsRequiredFields);

        private int PartTwo(ImmutableList<ImmutableDictionary<string, string>> input) =>
            input
                .Where(ContainsRequiredFields)
                .Where(HasAllFieldsValid)
                .Count();


        private bool ContainsRequiredFields(ImmutableDictionary<string, string> fields) =>
            fields.Count == 8 || (fields.Count == 7 && !fields.ContainsKey("cid"));

        private bool HasAllFieldsValid(ImmutableDictionary<string, string> fields) =>
            fields.All(kv => GetValidationFromFactory(kv.Key).Invoke(kv.Value));

        private Func<string, bool> GetValidationFromFactory(string fieldName) =>
            fieldName switch
            {
                "byr" => ValidateByr,
                "iyr" => ValidateIyr,
                "eyr" => ValidateEyr,
                "hgt" => ValidateHgt,
                "hcl" => ValidateHcl,
                "ecl" => ValidateEcl,
                "pid" => ValidatePid,
                "cid" => _ => true,
                _ => throw new Exception($"No such field: {fieldName}")
            };

        private bool ValidatePid(string arg) => arg.All(char.IsDigit) && arg.Length == 9;

        private bool ValidateEcl(string arg) =>
            new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(arg);

        private bool ValidateHcl(string arg) =>
            Regex.IsMatch(arg, @"#([0-9]|[a-f]){6}\z");

        private bool ValidateHgt(string arg) =>
            arg.EndsWith("cm")
            && int.TryParse(arg.Replace("cm", ""), out var cmHeight)
            && cmHeight >= 150 && cmHeight <= 193
            ||
            arg.EndsWith("in")
            && int.TryParse(arg.Replace("in", ""), out var inHeight)
            && inHeight >= 59 && inHeight <= 76;

        private bool ValidateEyr(string arg) =>
            ValidateYear(arg, 4, 2020, 2030);

        private bool ValidateIyr(string arg) =>
            ValidateYear(arg, 4, 2010, 2020);

        private bool ValidateByr(string arg) =>
            ValidateYear(arg, 4, 1920, 2002);

        private bool ValidateYear(string arg, int requiredDigitsCount, int lowerBound, int upperBound) =>
            arg.Length == requiredDigitsCount && int.TryParse(arg, out var year) && year >= lowerBound && year <= upperBound;
    }
}
