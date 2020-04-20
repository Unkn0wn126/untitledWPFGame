﻿using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
using Engine.Models.Factories.Scenes;
using Engine.Models.Scenes;
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TimeUtils;

namespace Engine.Models.Factories
{
    public static class SceneFactory
    {
        public static IScene CreateBattleScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler)
        {
            int objectSize = 2;
            int numOfObjectsInCell = 3;
            int cellSize = objectSize * numOfObjectsInCell;
            float baseCellXValue = (2 * objectSize) / (float)cellSize;
            float baseCellYValue = (2 * objectSize) / (float)cellSize;
            int numCellsX = (int)Math.Ceiling(baseCellXValue);
            int numCellsY = (int)Math.Ceiling(baseCellYValue);

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            ITransformComponent currTransform = new TransformComponent(new Vector2(1 * objectSize, 1 * objectSize), objectSize, objectSize, new Vector2(0, 0), 0);
            IGraphicsComponent current = new GraphicsComponent(ImgName.Player);

            uint currEntity = manager.AddEntity(currTransform);
            manager.AddComponentToEntity(currEntity, current);            
            
            ITransformComponent currTransform2 = new TransformComponent(new Vector2(2 * objectSize, 1 * objectSize), objectSize, objectSize, new Vector2(0, 0), 1);
            IGraphicsComponent current2 = new GraphicsComponent(ImgName.Enemy);

            uint currEntity2 = manager.AddEntity(currTransform2);
            manager.AddComponentToEntity(currEntity2, current2);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, grid);

            return scene;
        }

        private static MetaMapEntity[,] GenerateMetaMap(int numOfObjectsOnX, int numOfObjectsOnY)
        {
            MetaMapEntity[,] metaMap = new MetaMapEntity[numOfObjectsOnX, numOfObjectsOnY];
            ComponentState current;
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    if ((i == 0 || i == metaMap.GetLength(0) - 1) || (j == 0 || j == metaMap.GetLength(1) - 1))  // generate edges of the map
                    {
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent;
                        metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.Solid, Graphics = ImgName.Rock, Components = current, ZIndex = 1};
                    }
                    else
                    {
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent;
                        metaMap[i, j] = new MetaMapEntity { CollisionType = CollisionType.None, Graphics = ImgName.Dirt, Components = current, ZIndex = 0 };
                    }

                    
                }
            }

            return metaMap;
        }

        public static IScene CreateScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler, bool generateTheGuy, int numOfObjectsOnX, int numOfObjectsOnY)
        {
            int objectSize = 1;
            int numOfObjectsInCell = 4;
            int cellSize = objectSize * numOfObjectsInCell;
            float baseCellXValue = (numOfObjectsOnX * objectSize) / (float)cellSize;
            float baseCellYValue = (numOfObjectsOnY * objectSize) / (float)cellSize;
            int numCellsX = (int)Math.Ceiling(baseCellXValue);
            int numCellsY = (int)Math.Ceiling(baseCellYValue);

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, grid);

            MetaMapEntity[,] metaMap = GenerateMetaMap(numOfObjectsOnX, numOfObjectsOnY);

            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    EntityFactory.GenerateEntity(manager, metaMap[i, j].Components, metaMap[i, j].Graphics, metaMap[i, j].CollisionType, new Vector2(objectSize, objectSize), new Vector2(objectSize * i, objectSize * j), metaMap[i, j].ZIndex);
                }
            }

            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize, objectSize), objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Player);
            //_playerTransform = playerTransform;

            uint player = manager.AddEntity(playerTransform);
            //_player = player;
            manager.AddComponentToEntity(player, test);

            ICollisionComponent collision = new CollisionComponent(false, true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            scene.PlayerEntity = player;
            scene.PlayerTransform = playerTransform;

            manager.AddComponentToEntity(player, new PlayerMovementScript(gameTime, gameInputHandler, scene, player, 4 * objectSize));

            if (generateTheGuy)
            {
                SetupCharacter(scene, manager, objectSize, gameTime);
            }

            return scene;
        }

        private static void SetupCharacter(IScene scene, IEntityManager manager, float objectSize, GameTime gameTime)
        {
            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize * 2, objectSize * 2), objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Enemy);

            uint player = manager.AddEntity(playerTransform);
            manager.AddComponentToEntity(player, test);

            ICollisionComponent collision = new CollisionComponent(false, true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            IScriptComponent movementStrategy = new FollowPlayerScript(gameTime, scene, player, scene.PlayerEntity, 4 * objectSize);

            manager.AddComponentToEntity(player, movementStrategy);
        }
    }
}
