using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Engine.Models.Components;

namespace Engine.Coordinates
{
    [Serializable]
    public class Grid : ISpatialIndex
    {
        private int _numOfCellsOnX;
        private int _numOfCellsOnY;
        private int _cellSize;
        public List<uint>[][] Cells { get; set; }

        public Grid(int numOfCellsOnX, int numOfCellsOnY, int cellSize)
        {
            _numOfCellsOnX = numOfCellsOnX;
            _numOfCellsOnY = numOfCellsOnY;
            _cellSize = cellSize;

            Cells = new List<uint>[_numOfCellsOnX][];
            for (int i = 0; i < _numOfCellsOnX; i++)
            {
                Cells[i] = new List<uint>[numOfCellsOnY];
            }

            for (int i = 0; i < numOfCellsOnX; i++)
            {
                for (int j = 0; j < numOfCellsOnY; j++)
                {
                    Cells[i][j] = new List<uint>();
                }
            }
        }

        public void Add(uint unit, Vector2 position)
        {
            int cellX = (int)(position.X / _cellSize);
            int cellY = (int)(position.Y / _cellSize);
            Cells[cellX][cellY].Add(unit);
        }

        public List<uint> GetObjectsInRadius(ITransformComponent focus, int cellRadius)
        {
            int cellX = (int)(focus.Position.X / _cellSize);
            int cellY = (int)(focus.Position.Y / _cellSize);

            List<uint> gameObjects = new List<uint>();
            int minX = cellX - cellRadius > 0 ? cellX - cellRadius : 0;
            int maxX = cellX + cellRadius < _numOfCellsOnX ? cellX + cellRadius : _numOfCellsOnX;            
            
            int minY = cellY - cellRadius > 0 ? cellY - cellRadius : 0;
            int maxY = cellY + cellRadius < _numOfCellsOnY ? cellY + cellRadius : _numOfCellsOnY;

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    gameObjects.AddRange(Cells[i][j]);
                }
            }

            return gameObjects;
        }

        public void Move(uint unit, Vector2 oldPos, Vector2 newPos)
        {
            int oldCellX = (int)(oldPos.X / _cellSize);
            int oldCellY = (int)(oldPos.Y / _cellSize);

            int cellX = (int)(newPos.X / _cellSize);
            int cellY = (int)(newPos.Y / _cellSize);

            // If it didn't change cells, we're done.
            if (oldCellX == cellX && oldCellY == cellY) return;

            //// Unlink it from the list of its old cell.
            if (cellX < Cells.Length && cellX >= 0 && cellY < Cells[0].Length && cellY >= 0)
            {
                Cells[oldCellX][oldCellY].Remove(unit);

                // Add it back to the grid at its new cell.
                Add(unit, newPos);
            }

        }
    }
}
