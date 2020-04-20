using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
using Engine.Models.Factories.Entities;
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

        private static ComponentState[,] GenerateMetaMap(int numOfObjectsOnX, int numOfObjectsOnY)
        {
            ComponentState[,] metaMap = new ComponentState[numOfObjectsOnX, numOfObjectsOnY];
            ComponentState current;
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    if ((i == 0 || i == metaMap.GetLength(0) - 1) || (j == 0 || j == metaMap.GetLength(1) - 1))
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent | ComponentState.SolidCollision;
                    else
                        current = ComponentState.GraphicsComponent | ComponentState.TransformComponent;

                    metaMap[i, j] = current;
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

            ComponentState[,] metaMap = GenerateMetaMap(numOfObjectsOnX, numOfObjectsOnY);

            // This is gonna be in a factory...
            for (int i = 0; i < metaMap.GetLength(0); i++)
            {
                for (int j = 0; j < metaMap.GetLength(1); j++)
                {
                    ImgName currentName;
                    currentName = ImgName.Dirt;

                    EntityFactory.GenerateEntity(manager, metaMap[i, j], currentName, new Vector2(objectSize, objectSize), new Vector2(objectSize * i, objectSize * j), 0);
                }

                EntityFactory.GenerateEntity(manager, metaMap[i, 0], ImgName.Rock, new Vector2(objectSize, objectSize), new Vector2(objectSize * i, 0), 0);
                EntityFactory.GenerateEntity(manager, metaMap[i, numOfObjectsOnY - 1], ImgName.Rock, new Vector2(objectSize, objectSize), new Vector2(objectSize * i, (numOfObjectsOnY - 1) * objectSize), 0);
            }

            for (int j = 1; j < numOfObjectsOnY; j++)
            {
                EntityFactory.GenerateEntity(manager, metaMap[0, j], ImgName.Rock, new Vector2(objectSize, objectSize), new Vector2(0, objectSize * j), 0);
                EntityFactory.GenerateEntity(manager, metaMap[numOfObjectsOnX - 1, j], ImgName.Rock, new Vector2(objectSize, objectSize), new Vector2((numOfObjectsOnX - 1) * objectSize, objectSize * j), 0);
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
