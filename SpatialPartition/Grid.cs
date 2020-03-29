using System;
using System.Collections.Generic;
using System.Text;

namespace SpatialPartition
{
    public class Grid
    {
        private int _numOfCells;
        private int _cellSize;
        public Unit[,] Cells { get; set; }

        public Grid(int numOfCells, int cellSize)
        {
            _numOfCells = numOfCells;
            _cellSize = cellSize;

            Cells = new Unit[_numOfCells, _numOfCells];

            for (int i = 0; i < _numOfCells; i++)
            {
                for (int j = 0; j < _numOfCells; j++)
                {
                    Cells[i, j] = null;
                }
            }
        }

        public void Add(Unit unit)
        {
            int cellX = (int)(unit.X / _cellSize);
            int cellY = (int)(unit.Y / _cellSize);

            unit.Prev = null;
            unit.Next = Cells[cellX, cellY];
            Cells[cellX, cellY] = unit;

            if (unit.Next != null)
            {
                unit.Next.Prev = unit;
            }
        }
    }
}
