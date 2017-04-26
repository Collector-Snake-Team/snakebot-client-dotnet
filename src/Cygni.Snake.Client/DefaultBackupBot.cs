namespace Cygni.Snake.Client
{
    using System.Linq;

    public class DefaultBackupBot : SnakeBot
    {
        public DefaultBackupBot()
            : base("DefaultBackupBot", null)
        {
        }

        public override Direction GetNextMove()
        {
            return Directions.All.Select(d => new { Direction = d, DirectionalResult = Map.GetResultOfMyDirection(d) })
                             .OrderBy(r => r.DirectionalResult)
                             .First().Direction;
        }
    }
}