using System;
using System.IO;
using System.Linq;

namespace AoC2k20.Challenges
{
    public class Day11 : IAocChallenge
    {
        public string Evaluate()
        {
            var seatsMatrix = File.ReadAllLines("./input/Day11.txt")
                .Select(line => line.ToCharArray())
                .Select(seats =>
                    seats.Select(seat =>
                            seat switch
                            {
                                '#' => SeatKind.Occupied,
                                '.' => SeatKind.Floor,
                                'L' => SeatKind.Empty,
                                _ => throw new Exception($"Unknown seat kind: {seat}!")
                            })
                        .ToArray()
                )
                .ToArray();

            return
                $"Part One: {PartOne(seatsMatrix)}" + Environment.NewLine +
                $"Part Two: {PartTwo(seatsMatrix)}";
        }

        private int PartOne(SeatKind[][] seatsMatrix)
        {
            var seatChange = new PartOneSeatChange();

            return GetStabilizedSeatPlacements(seatsMatrix, seatChange)
                .SelectMany(colSeatKinds => colSeatKinds)
                .Count(seatKind => seatKind == SeatKind.Occupied);
        }

        private int PartTwo(SeatKind[][] seatsMatrix)
        {
            var seatChange = new PartTwoSeatChange();

            return GetStabilizedSeatPlacements(seatsMatrix, seatChange)
                .SelectMany(colSeatKinds => colSeatKinds)
                .Count(seatKind => seatKind == SeatKind.Occupied);
        }

        private SeatKind[][] GetStabilizedSeatPlacements(SeatKind[][] seatsMatrix, ISeatKindChange seatKindChange)
        {
            var currentIterationSeats = GetCopy(seatsMatrix);
            var alteredSeats = GetCopy(seatsMatrix);
            var hasSeatKindChange = true;

            while (hasSeatKindChange)
            {
                hasSeatKindChange = false;

                for (var rowIndex = 0; rowIndex < currentIterationSeats.Length; rowIndex++)
                {
                    for (var columnIndex = 0; columnIndex < currentIterationSeats[rowIndex].Length; columnIndex++)
                    {
                        var currentSeatKind = currentIterationSeats[rowIndex][columnIndex];
                        var newSeatKind = seatKindChange.GetNewSeatKind(currentIterationSeats, rowIndex, columnIndex);

                        if (currentSeatKind != newSeatKind)
                        {
                            hasSeatKindChange = true;
                        }

                        alteredSeats[rowIndex][columnIndex] = newSeatKind;
                    }
                }

                currentIterationSeats = GetCopy(alteredSeats);
            }

            return alteredSeats;
        }

        private static SeatKind[][] GetCopy(SeatKind[][] seatsMatrix) =>
            Enumerable.Range(0, seatsMatrix.Length)
                .Select(i =>
                    Enumerable.Range(0, seatsMatrix[i].Length)
                        .Select(j => seatsMatrix[i][j])
                        .ToArray()
                )
                .ToArray();

        public class PartOneSeatChange : ISeatKindChange
        {
            public SeatKind GetNewSeatKind(SeatKind[][] seatsMatrix, int rowIndex, int columnIndex)
            {
                var rowOffsets = Enumerable.Range(-1, 3);
                var columnOffsets = Enumerable.Range(-1, 3);

                var adjacentSeatsOffsets = rowOffsets
                    .SelectMany(rowOffset => columnOffsets.Select(columnOffset => (rowOffset, columnOffset)))
                    .Where(offsets =>
                        offsets != (0, 0)
                        && rowIndex + offsets.rowOffset >= 0 && rowIndex + offsets.rowOffset < seatsMatrix.Length
                        && columnIndex + offsets.columnOffset >= 0 && columnIndex + offsets.columnOffset < seatsMatrix[rowIndex].Length
                    )
                    .ToArray();

                var adjacentSeatKinds = adjacentSeatsOffsets
                    .Select(offsets =>
                        seatsMatrix[rowIndex + offsets.rowOffset][columnIndex + offsets.columnOffset]
                    )
                    .ToArray();

                return GetSeatKindBasedOnAdjacentSeats(seatsMatrix, rowIndex, columnIndex, adjacentSeatKinds);
            }

            private static SeatKind GetSeatKindBasedOnAdjacentSeats(
                SeatKind[][] seatsMatrix,
                int rowIndex,
                int columnIndex,
                SeatKind[] adjacentSeatKinds
            ) =>
                seatsMatrix[rowIndex][columnIndex] switch
                {
                    SeatKind.Empty
                        when adjacentSeatKinds.All(seatKind => seatKind != SeatKind.Occupied) =>
                        SeatKind.Occupied,
                    SeatKind.Occupied
                        when adjacentSeatKinds.Count(seatKind => seatKind == SeatKind.Occupied) >= 4 =>
                        SeatKind.Empty,
                    _ => seatsMatrix[rowIndex][columnIndex]
                };
        }

        public class PartTwoSeatChange : ISeatKindChange
        {
            public SeatKind GetNewSeatKind(SeatKind[][] seatsMatrix, int rowIndex, int columnIndex)
            {
                var rowOffsetDirections = new[] { -1, 0, 1 };
                var columnOffsetDirections = new[] { -1, 0, 1 };

                bool IsWithinMatrixRange(int currentRow, int currentColumn) =>
                    currentRow >= 0 && currentRow < seatsMatrix.Length
                                    && currentColumn >= 0 && currentColumn < seatsMatrix[currentRow].Length;

                var directions = rowOffsetDirections
                    .SelectMany(rowOffsetDirection =>
                        columnOffsetDirections.Select(columnOffsetDirection => (rowOffsetDirection, columnOffsetDirection)))
                    .Where(offsetDirections => offsetDirections != (0, 0));

                var adjacentSeatKinds = directions
                    .Select(offsetDirections =>
                    {
                        var currentRow = rowIndex + offsetDirections.rowOffsetDirection;
                        var currentColumn = columnIndex + offsetDirections.columnOffsetDirection;

                        while (
                            IsWithinMatrixRange(currentRow, currentColumn)
                            && seatsMatrix[currentRow][currentColumn] == SeatKind.Floor
                        )
                        {
                            currentRow += offsetDirections.rowOffsetDirection;
                            currentColumn += offsetDirections.columnOffsetDirection;
                        }

                        return IsWithinMatrixRange(currentRow, currentColumn)
                            ? seatsMatrix[currentRow][currentColumn]
                            : (SeatKind?)null;
                    })
                    .Where(s => s is not null)
                    .Cast<SeatKind>()
                    .ToArray();

                return GetSeatKindBasedOnAdjacentSeats(seatsMatrix, rowIndex, columnIndex, adjacentSeatKinds);
            }

            private static SeatKind GetSeatKindBasedOnAdjacentSeats(
                SeatKind[][] seatsMatrix,
                int rowIndex,
                int columnIndex,
                SeatKind[] adjacentSeatKinds
            ) =>
                seatsMatrix[rowIndex][columnIndex] switch
                {
                    SeatKind.Empty
                        when adjacentSeatKinds.All(seatKind => seatKind != SeatKind.Occupied) =>
                        SeatKind.Occupied,
                    SeatKind.Occupied
                        when adjacentSeatKinds.Count(seatKind => seatKind == SeatKind.Occupied) >= 5 =>
                        SeatKind.Empty,
                    _ => seatsMatrix[rowIndex][columnIndex]
                };
        }
    }

    public interface ISeatKindChange
    {
        SeatKind GetNewSeatKind(SeatKind[][] seatsMatrix, int rowIndex, int columnIndex);
    }

    public enum SeatKind
    {
        Occupied,
        Floor,
        Empty
    }
}
