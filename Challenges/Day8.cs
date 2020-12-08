using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day8 : IAocChallenge
    {
        public string Evaluate()
        {
            var instructions = File.ReadAllLines("./input/Day8.txt")
                .Select(Instruction.FromCode)
                .ToImmutableArray();

            return
                $"Part One: {PartOne(instructions)}" + Environment.NewLine +
                $"Part Two: {PartTwo(instructions)}";
        }

        private int PartOne(ImmutableArray<Instruction> instructions) => GetAccAfterFirstIteration(instructions);

        private int PartTwo(ImmutableArray<Instruction> instructions) => GetAccAfterValidTermination(instructions);

        private static int GetAccAfterFirstIteration(ImmutableArray<Instruction> instructions)
        {
            var (acc, _) = Execute(instructions);
            return acc;
        }

        private int GetAccAfterValidTermination(ImmutableArray<Instruction> instructions)
        {
            var possibleSwitchPoints = instructions
                .Select((instruction, index) => new { instruction, index })
                .Where(x =>
                    x.instruction.Operation == Operation.Jmp
                    || x.instruction.Operation == Operation.Nop)
                .Select(x =>
                    new
                    {
                        SwitchIndex = x.index,
                        NewOperation = x.instruction.Operation == Operation.Jmp ? Operation.Nop : Operation.Jmp
                    }
                )
                .ToImmutableList();

            foreach (var switchPoint in possibleSwitchPoints)
            {
                var (acc, terminated) = Execute(
                    instructions
                        .Select((instruction, index) =>
                            index == switchPoint.SwitchIndex
                                ? instruction.With(switchPoint.NewOperation)
                                : instruction
                        )
                        .ToImmutableArray()
                );

                if (terminated) 
                    return acc;
            }

            return -1;
        }

        private static (int acc, bool terminated) Execute(ImmutableArray<Instruction> instructions)
        {
            var lineExecutions = new bool[instructions.Length];

            var acc = 0;
            var currentLine = 0;

            bool Terminated() => currentLine == instructions.Length;

            while (!Terminated() && !lineExecutions[currentLine])
            {
                var (operation, argument) = instructions[currentLine];

                var instructionProcessor = InstructionProcessorFactory.Get(operation);

                var (accAfter, lineAfter) = instructionProcessor.Evaluate(argument, acc, currentLine);

                lineExecutions[currentLine] = true;
                (acc, currentLine) = (accAfter, lineAfter);
            }

            return (acc, Terminated());
        }

        #region Instruction Processors

        public static class InstructionProcessorFactory
        {
            public static IInstructionProcessor Get(Operation operation) =>
                operation switch
                {
                    Operation.Nop => new NopProcessor(),
                    Operation.Acc => new AccProcessor(),
                    Operation.Jmp => new JmpProcessor(),
                    _ => throw new ArgumentException("Invalid Operation Exception!")
                };
        }

        public class NopProcessor : IInstructionProcessor
        {
            public (int accAfter, int lineAfter) Evaluate(int argument, int acc, int currentLine)
            {
                return (acc, currentLine + 1);
            }
        }

        public class JmpProcessor : IInstructionProcessor
        {
            public (int accAfter, int lineAfter) Evaluate(int argument, int acc, int currentLine)
            {
                if (argument == 0) throw new ArgumentException();
                return (acc, currentLine + argument);
            }
        }

        public class AccProcessor : IInstructionProcessor
        {
            public (int accAfter, int lineAfter) Evaluate(int argument, int acc, int currentLine)
            {
                return (acc + argument, currentLine + 1);
            }
        }

        public interface IInstructionProcessor
        {
            (int accAfter, int lineAfter) Evaluate(int argument, int acc, int currentLine);
        }

        #endregion


        //TODO: change to record
        public class Instruction
        {
            public Operation Operation { get; }
            public int Argument { get; }

            private Instruction(Operation operation, int argument)
            {
                Operation = operation;
                Argument = argument;
            }

            public void Deconstruct(out Operation operation, out int argument)
            {
                operation = Operation;
                argument = Argument;
            }

            public static Instruction FromCode(string instructionCode)
            {
                var instructionWithArgument = instructionCode.Split(" ");

                if (!Enum.TryParse<Operation>(instructionWithArgument[0], true, out var operation))
                {
                    throw new Exception($"Not known operation: {instructionWithArgument[1]}");
                }

                return new Instruction(operation, Convert.ToInt32(instructionWithArgument[1]));
            }

            public Instruction With(Operation operation) => new Instruction(operation, Argument);
        }

        public enum Operation
        {
            Nop,
            Acc,
            Jmp
        }
    }
}
