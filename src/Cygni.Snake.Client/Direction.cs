namespace Cygni.Snake.Client
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class Directions
    {
        public static Direction[] All = { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
    }
}