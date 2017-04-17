namespace Cygni.Snake.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class TileGridBuilder
    {
        private static readonly Direction[] AllDirections = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
        private readonly IDictionary<MapCoordinate, TileType> _tileGrid;

        public TileGridBuilder(int width, int height)
        {
            _tileGrid = new Dictionary<MapCoordinate, TileType>();

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                _tileGrid[new MapCoordinate(x, y)] = TileType.Empty;
        }

        public TileGridBuilder WithFood(IEnumerable<MapCoordinate> foodPositions)
        {
            foreach (var foodPosition in foodPositions)
                _tileGrid[foodPosition] = TileType.Food;

            return this;
        }

        public TileGridBuilder WithObstacles(IEnumerable<MapCoordinate> obstaclePositions)
        {
            foreach (var obstaclePosition in obstaclePositions)
                _tileGrid[obstaclePosition] = TileType.Obstacle;

            return this;
        }

        public TileGridBuilder WithMySnake(SnakePlayer mySnake)
        {
            foreach (var position in mySnake.Positions)
                _tileGrid[position] = TileType.MyBody;

            _tileGrid[mySnake.HeadPosition] = TileType.MyHead;
            _tileGrid[mySnake.TailPosition] = TileType.MyTail;

            return this;
        }

        public TileGridBuilder WithOpponentSnakes(IEnumerable<SnakePlayer> snakes)
        {
            foreach (var opponentSnake in snakes.Where(s => s.IsAlive))
            {
                foreach (var position in opponentSnake.Positions)
                    _tileGrid[position] = TileType.OpponentBody;

                _tileGrid[opponentSnake.HeadPosition] = TileType.OpponentHead;
                _tileGrid[opponentSnake.TailPosition] = TileType.OpponentTail;
            }

            return this;
        }

        public TileGridBuilder WithOpponentHeadNeighbors(IEnumerable<SnakePlayer> snakes)
        {
            foreach (var opponentSnake in snakes.Where(s => s.IsAlive))
            {
                AllDirections.Select(opponentSnake.HeadPosition.GetDestination)
                             .ForEach(TryMarkAsOpponentHeadNeighbor);

                var twoStepsInFrontOfHead = opponentSnake.HeadPosition
                                                         .GetDestination(opponentSnake.CurrentDirection)
                                                         .GetDestination(opponentSnake.CurrentDirection);
                TryMarkAsOpponentHeadNeighbor(twoStepsInFrontOfHead);
            }

            return this;
        }

        private void TryMarkAsOpponentHeadNeighbor(MapCoordinate mapCoordinate)
        {
            if (_tileGrid.ContainsKey(mapCoordinate) && (_tileGrid[mapCoordinate] == TileType.Empty || _tileGrid[mapCoordinate] == TileType.Food))
                _tileGrid[mapCoordinate] = TileType.OpponentHeadNeighbor;
        }

        public IDictionary<MapCoordinate, TileType> Build()
        {
            return _tileGrid;
        }
    }
}