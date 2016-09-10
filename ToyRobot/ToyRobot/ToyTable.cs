/// <summary>
/// ToyTable Class
/// </summary>
namespace ToyRobot
{
    using System;

    public class ToyTable
    {
        CoordinateXY _tableBoundary = null;
        
        // y axis of the table initialize to 5 units
        public CoordinateXY TableBoundary
        {
            get { return _tableBoundary; }
        }

        public ToyTable(int height = 5, int width = 5)
        {
            SetTableDimensions(height, width);
        }

        private void SetTableDimensions(int height, int width)
        {
            if (height <= 0 || width <= 0)
            {
                throw new ArgumentException(
                    $"Invalid table dimensions: height {height}, width {width}");
            }

            this._tableBoundary = new CoordinateXY(height - 1 , width - 1);
        }

        // validate if robot placement is valid
        public bool IsValidPlacement(CoordinateXY targetLocation)
        {
            // if the coordinate  is out side of x: 0 -> X 
            if (targetLocation.X < 0 || 
                targetLocation.Y < 0 || 
                targetLocation.Y > this.TableBoundary.Y || 
                targetLocation.X > this.TableBoundary.X)
            {
                return false;
            }

            return true;
        }
    }   
}