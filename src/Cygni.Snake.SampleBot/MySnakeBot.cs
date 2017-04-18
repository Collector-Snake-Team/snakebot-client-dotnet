using Cygni.Snake.Client;

namespace Cygni.Snake.SampleBot
{
    public class MySnakeBot : SnakeBot
    {
        public MySnakeBot(string name) : base(name)
        {
        }

        public override Direction GetNextMove()
        {
            return Map.MySnake.CurrentDirection;
        }
    }
}