using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Cygni.Snake.Client
{
    public class Map
    {
        public Map(int width, int height, int worldTick, SnakePlayer mySnake, IEnumerable<SnakePlayer> snakeInfos, IEnumerable<MapCoordinate> foodPositions, IEnumerable<MapCoordinate> obstaclePositions)
        {
            Tick = worldTick;
            MySnake = mySnake;
            FoodPositions = new HashSet<MapCoordinate>(foodPositions);
            ObstaclePositions = new HashSet<MapCoordinate>(obstaclePositions);
            Snakes = snakeInfos.ToList();
            OpponentSnakes = Snakes.Where(s => s.Id != MySnake.Id).ToList();

            TileGrid = new TileGridBuilder(width, height)
                .WithFood(FoodPositions)
                .WithObstacles(ObstaclePositions)
                .WithMySnake(MySnake)
                .WithOpponentSnakes(OpponentSnakes)
                .WithOpponentHeadNeighbors(OpponentSnakes)
                .Build();

            OpponentsTailsPositions = OpponentSnakes.Where(s => s.IsAlive)
                                                   .ToDictionary(s => s.TailPosition, s => s);

            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public int Tick { get; }

        public IDictionary<MapCoordinate, TileType> TileGrid { get; }

        public TileType this[MapCoordinate mapCoordinate] => TileGrid.ContainsKey(mapCoordinate) ? TileGrid[mapCoordinate] : TileType.OutOfBounds;

        public bool IsDanger(MapCoordinate mapCoordinate) => this[mapCoordinate].IsDanger();

        public bool IsSafe(MapCoordinate mapCoordinate) => this[mapCoordinate].IsSafe();

        public IReadOnlyList<SnakePlayer> Snakes { get; }

        public IReadOnlyList<SnakePlayer> OpponentSnakes { get; }

        public SnakePlayer MySnake { get; }

        public SnakePlayer GetSnake(string id)
        {
            return Snakes.FirstOrDefault(s => s.Id.Equals(id, StringComparison.Ordinal));
        }

        public ISet<MapCoordinate> FoodPositions { get; }

        public ISet<MapCoordinate> ObstaclePositions { get; }

        public IReadOnlyDictionary<MapCoordinate, SnakePlayer> OpponentsTailsPositions { get; }

        public IEnumerable<MapCoordinate> SnakeHeads
        {
            get
            {
                return Snakes.Where(snake => snake.IsAlive)
                             .Select(snake => snake.HeadPosition);
            }
        }

        public IEnumerable<MapCoordinate> SnakeBodies
        {
            get
            {
                return Snakes.Where(s => s.IsAlive).SelectMany(s => s.Body);
            }
        }

        public IEnumerable<MapCoordinate> SnakeParts
        {
            get
            {
                return Snakes.SelectMany(s => s.Positions);
            }
        }

        public DirectionalResult GetResultOfMyDirection(Direction dir)
        {
            var target = MySnake.HeadPosition.GetDestination(dir);

            switch (this[target])
            {
                case TileType.Food:
                    return DirectionalResult.Food;
                case TileType.Empty:
                    return DirectionalResult.Nothing;
                case TileType.OpponentHeadNeighbor:
                    return DirectionalResult.Danger;
                case TileType.OpponentTail:
                    return Tick % 3 == 0 ? DirectionalResult.TailNibble : DirectionalResult.Nothing;
                default:
                    return DirectionalResult.Death;
            }
        }

        public DirectionalResult GetResultOfDirection(string playerId, Direction dir)
        {
            var snake = GetSnake(playerId);
            if (snake == null)
            {
                throw new ArgumentException($"No snake with id: {playerId}");
            }

            var target = snake.HeadPosition.GetDestination(dir);

            switch (this[target])
            {
                case TileType.Food:
                    return DirectionalResult.Food;
                case TileType.Empty:
                case TileType.OpponentHeadNeighbor:
                    return DirectionalResult.Nothing;
                default:
                    return DirectionalResult.Death;
            }
        }

        public bool IsObstacle(MapCoordinate coordinate)
        {
            return this[coordinate] == TileType.Obstacle;
        }

        public bool IsSnake(MapCoordinate coordinate)
        {
            return SnakeParts.Contains(coordinate);
        }

        public bool IsFood(MapCoordinate coordinate)
        {
            return this[coordinate] == TileType.Food;
        }

        public bool AbleToUseDirection(string playerId, Direction dir)
        {
            return GetResultOfDirection(playerId, dir).Equals(DirectionalResult.Death) == false;
        }

        public static Map FromJson(string json, string playerId)
        {
            return FromJson(JObject.Parse(json), playerId);
        }

        public static Map FromJson(JObject json, string playerId)
        {
            int width = (int)json["width"];
            int height = (int)json["height"];
            int tick = (int)json["worldTick"];            

            var snakes = json["snakeInfos"].Select(token =>
            {
                string name = (string) token["name"];
                string id = (string) token["id"];
                int points = (int) token["points"];
                var positions = token["positions"].Select(i => MapCoordinate.FromIndex((int) i, width));
                return new SnakePlayer(id, name, points, positions);
            }).ToList();

            var mySnake = snakes.FirstOrDefault(s => s.Id.Equals(playerId));

            var foods = json["foodPositions"].Select(i => MapCoordinate.FromIndex((int) i, width));
            var obstacles = json["obstaclePositions"].Select(i => MapCoordinate.FromIndex((int) i, width));
            return new Map(width, height, tick, mySnake, snakes, foods, obstacles);
        }
    }
}