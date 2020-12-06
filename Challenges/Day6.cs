using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2k20.Challenges
{
    public class Day6 : IAocChallenge
    {
        public string Evaluate()
        {
            var groups = File.ReadAllText("./input/Day6.txt")
                .Split("\n\n")
                .Select(group => 
                    new Group(group
                            .Split("\n")
                            .Select(personAnswers => new Person(personAnswers.ToCharArray()))
                            .ToImmutableArray())
                )
                .ToImmutableArray();

            return
                $"Part One: {PartOne(groups)}" + Environment.NewLine +
                $"Part Two: {PartTwo(groups)}";
        }

        private int PartOne(ImmutableArray<Group> groups) =>
            groups
                .Select(group => 
                    group.People
                        .SelectMany(p => p.Answers)
                        .ToHashSet()
                        .Count
                )
                .Sum();

        private int PartTwo(ImmutableArray<Group> groups) =>
            groups
                .Select(group =>
                    group.People
                        .SelectMany(p => p.Answers)
                        .GroupBy(c => c)
                        .Count(g => g.Count() == group.People.Length))
                .Sum();
    }

    public class Group
    {
        public ImmutableArray<Person> People { get; }

        public Group(ImmutableArray<Person> people)
        {
            People = people;
        }
    }

    public class Person
    {
        public ImmutableArray<char> Answers { get; }

        public Person(char[] answers)
        {
            Answers = answers.ToImmutableArray();
        }
    }
}
