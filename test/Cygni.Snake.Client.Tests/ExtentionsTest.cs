// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentionsTest.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Cygni.Snake.Client.Tests
{
    using System.Linq;

    using Xunit;

    public class ExtentionsTest
    {
        [Fact]
        public void Neighbors_of_a_coordinate_are_the_adjacent_coordinates()
        {
            var coordiate = new MapCoordinate(34, 78);
            var neighbours = coordiate.Neighbours().ToList();

            Assert.Equal(4, neighbours.Count);
            Assert.Contains(new MapCoordinate(34, 79), neighbours);
            Assert.Contains(new MapCoordinate(34, 77), neighbours);
            Assert.Contains(new MapCoordinate(33, 78), neighbours);
            Assert.Contains(new MapCoordinate(35, 78), neighbours);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Neighbors_of_a_distance_returns_all_neighbours_that_has_that_manhattan_distance_from_the_coordinate(int distance)
        {
            var start = new MapCoordinate(40, 65);
            var neighbours = start.NeighboursOfDistance(distance).ToList();

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    var mapCoordinate = new MapCoordinate(x, y);
                    if (mapCoordinate.GetManhattanDistanceTo(start) == distance)
                    {
                        Assert.Contains(mapCoordinate, neighbours);
                    }
                    else
                    {
                        Assert.DoesNotContain(mapCoordinate, neighbours);
                    }
                }
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void Neighbors_within_a_distance_returns_all_neighbours_that_has_that_manhattan_distance_or_nearer_from_the_coordinate(int distance)
        {
            var start = new MapCoordinate(40, 65);
            var neighbours = start.NeighboursWithinDistance(distance).ToList();

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    var mapCoordinate = new MapCoordinate(x, y);
                    var manhattanDistance = mapCoordinate.GetManhattanDistanceTo(start);
                    if (manhattanDistance <= distance && manhattanDistance > 0)
                    {
                        Assert.Contains(mapCoordinate, neighbours);
                    }
                    else
                    {
                        Assert.DoesNotContain(mapCoordinate, neighbours);
                    }
                }
            }
        }
    }
}