using System;

namespace SpatialPartition
{
    public class Unit
    {
        public Grid Grid { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Unit Prev { get; set; }
        public Unit Next { get; set; }
        public Unit(Grid grid, double x, double y)
        {
            Grid = grid;
            X = x;
            Y = y;
            Prev = null;
            Next = null;

            grid.Add(this);
        }
    }
}
