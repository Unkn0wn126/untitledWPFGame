using System.Collections.Generic;
using System.Numerics;
using Engine.Models.Components;

namespace Engine.Coordinates
{
    public class Grid : ISpatialIndex
    {
        private readonly int _numOfCellsOnX;
        private readonly int _numOfCellsOnY;
        private readonly int _cellSize;
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

        /// <summary>
        /// Adds the entity to the grid
        /// based on its current position
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="position"></param>
        public void Add(uint unit, Vector2 position)
        {
            int cellX = (int)(position.X / _cellSize);
            int cellY = (int)(position.Y / _cellSize);
            Cells[cellX][cellY].Add(unit);
        }

        /// <summary>
        /// Gets all of the entities in a given
        /// radius from the provided position
        /// </summary>
        /// <param name="focus"></param>
        /// <param name="cellRadius"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Moves the given entity from
        /// its old position to a new one
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="oldPos"></param>
        /// <param name="newPos"></param>
        public void Move(uint unit, Vector2 oldPos, Vector2 newPos)
        {
            int oldCellX = (int)(oldPos.X / _cellSize);
            int oldCellY = (int)(oldPos.Y / _cellSize);

            int cellX = (int)(newPos.X / _cellSize);
            int cellY = (int)(newPos.Y / _cellSize);

            // If it didn't change cells, we're done.
            if (oldCellX == cellX && oldCellY == cellY) return;

            // Unlink it from the list of its old cell.
            if (cellX < Cells.Length && cellX >= 0 && cellY < Cells[0].Length && cellY >= 0)
            {
                Cells[oldCellX][oldCellY].Remove(unit);

                // Add it back to the grid at its new cell.
                Add(unit, newPos);
            }

        }

        /// <summary>
        /// Removes the given entity
        /// from this grid
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="entityTransform"></param>
        public void Remove(uint unit, ITransformComponent entityTransform)
        {
            int cellX = (int)(entityTransform.Position.X / _cellSize);
            int cellY = (int)(entityTransform.Position.Y / _cellSize);

            Cells[cellX][cellY].Remove(unit);
        }
    }
}
