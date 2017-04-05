namespace Cygni.Snake.Client
{
    using System;

    public enum TileType
    {
        OutOfBounds,
        Empty,
        Food,
        Obstacle,
        OpponentHead,
        OpponentHeadNeighbor,
        OpponentTail,
        OpponentBody,
        MyHead,
        MyTail,
        MyBody
    }

    public static class TileTypeExtentions
    {
        public static bool IsDanger(this TileType tileType)
        {
            return !tileType.IsSafe();
        }

        public static bool IsSafe(this TileType tileType)
        {
            switch (tileType)
            {
                case TileType.OutOfBounds:
                case TileType.Obstacle:
                case TileType.OpponentHead:
                case TileType.OpponentHeadNeighbor:
                case TileType.OpponentBody:
                case TileType.MyBody:
                case TileType.MyHead:
                case TileType.MyTail:
                    return false;
                case TileType.OpponentTail:
                case TileType.Empty:
                case TileType.Food:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}