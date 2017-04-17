namespace Cygni.Snake.Client
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents an object that contains logic for choosing which 
    /// direction a <see cref="SnakePlayer"/> should move given the
    /// state of the world.
    /// </summary>
    public abstract class SnakeBot
    {
        /// <summary>
        /// Initializes a new instanse of the <see cref="SnakeBot"/> class
        /// with the specified name.
        /// </summary>
        /// <param name="name">The specified name.</param>
        protected SnakeBot(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of this <see cref="SnakeBot"/> instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a value indicating if this SnakeBot automatically starts a game on
        /// successful player registration.
        /// </summary>
        public bool AutoStart { get; set; }

        protected Map Map { get; private set; }

        /// <summary>
        /// When overriden in a derived class, gets the <see cref="Direction"/>
        /// in which this bot wants to move based on the specified <see cref="Map"/> in the Map property.
        /// </summary>
        /// <returns>The desired direction.</returns>
        public abstract Direction GetNextMove();

        public Direction GetNextMove(Map map)
        {
            Map = map;
            try
            {
                var nextMove = GetNextMove();

                if (Map.GetResultOfMyDirection(nextMove) != DirectionalResult.Death)
                    return nextMove;

                return SimpleNonDeathMove ?? nextMove;
            }
            catch (Exception)
            {
                return SimpleNonDeathMove ?? Map.MySnake.CurrentDirection;
            }
        }

        protected virtual Direction? SimpleNonDeathMove
        {
            get
            {
                return Directions.All.Select(d => new { Direction = d, DirectionalResult = Map.GetResultOfMyDirection(d) })
                                 .FirstOrDefault(r => r.DirectionalResult != DirectionalResult.Death)
                                 ?.Direction;
            }
        }
    }
}