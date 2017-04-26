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
            BackupBot = new DefaultBackupBot();
        }

        /// <summary>
        /// Initializes a new instanse of the <see cref="SnakeBot"/> class
        /// with the specified name.
        /// </summary>
        /// <param name="name">The specified name.</param>
        /// <param name="backupBot">A backup algorithm to use when the first one results in certain death.</param>
        protected SnakeBot(string name, SnakeBot backupBot)
        {
            Name = name;
            BackupBot = backupBot;
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

        protected SnakeBot BackupBot { get; }

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

                if (BackupBot != null && Map.GetResultOfMyDirection(nextMove) == DirectionalResult.Death)
                    return BackupBot.GetNextMove(Map);

                return nextMove;
            }
            catch (Exception)
            {
                return BackupBot?.GetNextMove(Map) ?? Map.MySnake.CurrentDirection;
            }
        }
    }
}