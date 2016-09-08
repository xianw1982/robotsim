/// <summary>
/// Robot class
/// </summary>
namespace ToyRobot
{
    using System;
    using System.IO;

    public enum Direction
    {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }


    public enum Commands
    {
        PLACE,
        REPORT,
        LEFT,
        RIGHT,
        MOVE
    }

    public class Robot
    {
        private Table _table = null;
        private Direction _currentDirection = Direction.NORTH;
        private int _x = -1;
        private int _y = -1;

        public Robot(Table table, int x, int y, Direction f = Direction.NORTH)
        {
            if (table == null)
            {
                throw new ArgumentException("table can not be null");
            }

            if (!table.IsValidPlacement(x, y))
            {
                throw new ArgumentException(
                    string.Format(
                    "Invalid table placement : {0}, {1} on a table with dimensions {2}, {3}",
                    x,
                    y,
                    table.Width,
                    table.Height));
            }

            this._x = x;
            this._y = y;
            this._currentDirection = f;
            this._table = table;
        }

        public void Report(StreamWriter sw)
        {
            sw.WriteLine(string.Format(
                    "{0},{1},{2}",
                    this._x,
                    this._y,
                    this._currentDirection.ToString()));

        }

        /// <summary>
        /// Move function will move the toy robot one unit forward in the direction it is currently facing.
        /// </summary>
        /// <returns>true if move was successful, false otherwise</returns>
        public bool Move()
        {
            int newX = this._x;
            int newY = this._y;

            switch (_currentDirection)
            {
                case (Direction.NORTH):
                    newY++;
                    break;
                case (Direction.WEST):
                    newX--;
                    break;
                case (Direction.SOUTH):
                    newY--;
                    break;
                case (Direction.EAST):
                    newX++;
                    break;
                default:
                    throw new NotSupportedException(
                        string.Format(
                            "Direction {0} is not supported in the move function.", 
                            _currentDirection.ToString()));
            }

            if (_table.IsValidPlacement(newX, newY))
            {
                this._x = newX;
                this._y = newY;
                return true;
            }else
            {
                return false;
            }
        }

        public void Turn(bool isLeftTurn)
        {
            switch (_currentDirection)
            {
                case (Direction.NORTH):
                    _currentDirection = isLeftTurn ? Direction.WEST : Direction.EAST;
                    break;
                case (Direction.WEST):
                    _currentDirection = isLeftTurn ? Direction.SOUTH : Direction.NORTH;
                    break;
                case (Direction.SOUTH):
                    _currentDirection = isLeftTurn ? Direction.EAST : Direction.WEST;
                    break;
                case (Direction.EAST):
                    _currentDirection = isLeftTurn ? Direction.NORTH : Direction.SOUTH;
                    break;
                default:
                    throw new NotSupportedException(
                        string.Format(
                            "Direction {0} is not supported in the current turn function.",
                            _currentDirection.ToString()));
            }
        }

    }
}