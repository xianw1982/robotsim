namespace ToyRobot
{
    using System;

    /// <summary>
    /// enumerator containing the supported directions
    /// </summary>
    public enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }

    /// <summary>
    /// class for storing the XY coordinates
    /// </summary>
    public class CoordinateXY
    {
        private int _x;
        private int _y;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        public CoordinateXY(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X Axis coordinate
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Y Axis coordinate
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Move the coordinate in a certain direction n increments
        /// </summary>
        /// <param name="direction">direction to move</param>
        /// <param name="increment">number of increments</param>
        public void Move(Direction direction, int increment)
        {
            switch (direction)
            {
                case (Direction.NORTH):
                    this._y += increment;
                    break;
                case (Direction.WEST):
                    this._x -= increment;
                    break;
                case (Direction.SOUTH):
                    this._y -= increment;
                    break;
                case (Direction.EAST):
                    this._x += increment;
                    break;
                default:
                    throw new NotSupportedException(
                            $"Direction {direction} is not supported");
            }
        }

        /// <summary>
        /// override tostring function
        /// </summary>
        /// <returns>serialized coordinate</returns>
        public override string ToString()
        {
            return $"{this.X},{this.Y}";
        }
    }
}
