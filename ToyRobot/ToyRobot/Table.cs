/// <summary>
/// Table Class
/// </summary>

namespace ToyRobot
{
    using System;

    public class Table
    {
        private int _y = -1;
        private int _x = -1;

        // y axis of the table initialize to 5 units
        public int Height
        {
            get { return _y;}
        }
        // x axis of the table initialize to 5 units
        public int Width
        {
            get { return _x; }
        }

        public Table(int height = 5, int width = 5)
        {
            SetTableDimensions(height, width);
        }

        public void SetTableDimensions(int height, int width)
        {
            if (height <= 0 || width <= 0)
            {
                throw new ArgumentException(
                    string.Format(
                    "Invalid table dimensions: height {0}, width {1}",
                    height,
                    width));
            }

            this._y = height;
            this._x = width;
        }

        // validate if robot placement is valid
        public bool IsValidPlacement(int x, int y)
        {
            // if the coordinate  is out side of x: 0 -> X 
            if (x < 0 || y < 0 || y >= this.Height || x >= this.Width)
            {
                return false;
            }
            return true;
        }
    }
}