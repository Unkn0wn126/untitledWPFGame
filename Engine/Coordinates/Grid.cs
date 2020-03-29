using Engine.Models.GameObjects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models.Components;

namespace Engine.Coordinates
{
    public class Grid
    {
        private int _numOfCells;
        private int _cellSize;
        public List<IGameObject>[][] Cells { get; set; }

        public Grid(int numOfCells, int cellSize)
        {
            _numOfCells = numOfCells;
            _cellSize = cellSize;

            Cells = new List<IGameObject>[_numOfCells][];
            for (int i = 0; i < _numOfCells; i++)
            {
                Cells[i] = new List<IGameObject>[_numOfCells];
            }

            for (int i = 0; i < _numOfCells; i++)
            {
                for (int j = 0; j < _numOfCells; j++)
                {
                    Cells[i][j] = new List<IGameObject>();
                }
            }
        }

        public void Add(IGameObject unit)
        {
            int cellX = (int)(unit.Position.X / _cellSize);
            int cellY = (int)(unit.Position.Y / _cellSize);
            Cells[cellX][cellY].Add(unit);
        }

        public List<IGameObject> GetObjectsInRadius(IGameObject focus, int cellRadius)
        {
            int cellX = (int)(focus.Position.X / _cellSize);
            int cellY = (int)(focus.Position.Y / _cellSize);

            List<IGameObject> gameObjects = new List<IGameObject>();
            int minX = cellX - cellRadius > 0 ? cellX - cellRadius : 0;
            int maxX = cellX + cellRadius < _numOfCells ? cellX + cellRadius : _numOfCells;            
            
            int minY = cellY - cellRadius > 0 ? cellY - cellRadius : 0;
            int maxY = cellY + cellRadius < _numOfCells ? cellY + cellRadius : _numOfCells;
            //Parallel.For(minX, maxX, index =>
            //{
            //    Parallel.For(minY, maxY, innerIndex =>
            //    {
            //        gameObjects.AddRange(Cells[index][innerIndex]);
            //    });
            //});
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    gameObjects.AddRange(Cells[i][j]);
                }
            }

            return gameObjects;
        }

        public List<IGraphicsComponent> GetGraphicsComponentsInRadius(IGameObject focus, int cellRadius)
        {
            int cellX = (int)(focus.Position.X / _cellSize);
            int cellY = (int)(focus.Position.Y / _cellSize);

            List<IGraphicsComponent> gameObjects = new List<IGraphicsComponent>();
            int minX = cellX - cellRadius > 0 ? cellX - cellRadius : 0;
            int maxX = cellX + cellRadius < _numOfCells ? cellX + cellRadius : _numOfCells;            
            
            int minY = cellY - cellRadius > 0 ? cellY - cellRadius : 0;
            int maxY = cellY + cellRadius < _numOfCells ? cellY + cellRadius : _numOfCells;
            //Parallel.For(minX, maxX, index =>
            //{
            //    Parallel.For(minY, maxY, innerIndex =>
            //    {
            //        gameObjects.AddRange(Cells[index][innerIndex]);
            //    });
            //});
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    Cells[i][j].ForEach(x => gameObjects.Add(x.GraphicsComponent));
                }
            }

            return gameObjects;
        }

        public void Move(IGameObject unit, float x, float y)
        {
            int oldCellX = (int)(unit.Position.X / _cellSize);
            int oldCellY = (int)(unit.Position.Y / _cellSize);

            int cellX = (int)(x / _cellSize);
            int cellY = (int)(y / _cellSize);

            Vector2 newPos = new Vector2(x, y);
            unit.Position = newPos;

            // If it didn't change cells, we're done.
            if (oldCellX == cellX && oldCellY == cellY) return;

            // Unlink it from the list of its old cell.
            Cells[cellX][cellY].Remove(unit);

            // Add it back to the grid at its new cell.
            Add(unit);
        }
    }
}
