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

            ComponentState req = ComponentState.GraphicsComponent | ComponentState.TransformComponent;
            ComponentState req2 = ComponentState.GraphicsComponent | ComponentState.TransformComponent | ComponentState.CollisionComponent;

            // This is gonna be in a factory...
            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                for (int j = 0; j < numOfObjectsOnY; j++)
                {
                    ImgName currentName;
                    currentName = ImgName.Dirt;

                    EntityFactory.GenerateEntity(manager, req, objectSize, objectSize, objectSize * i, objectSize * j, 0, currentName, gameTime);
                }

                EntityFactory.GenerateEntity(manager, req2, objectSize, objectSize, objectSize * i, 0, 0, ImgName.Rock, gameTime);
                EntityFactory.GenerateEntity(manager, req2, objectSize, objectSize, objectSize * i, (numOfObjectsOnY - 1) * objectSize, 0, ImgName.Rock, gameTime);
            }

            for (int j = 1; j < numOfObjectsOnY; j++)
            {
                EntityFactory.GenerateEntity(manager, req2, objectSize, objectSize, 0, j * objectSize, 0, ImgName.Rock, gameTime);
                EntityFactory.GenerateEntity(manager, req2, objectSize, objectSize, (numOfObjectsOnX - 1) * objectSize, j * objectSize, 0, ImgName.Rock, gameTime);
            }

            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize, objectSize), objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Player);
            //_playerTransform = playerTransform;

            uint player = manager.AddEntity(playerTransform);
            //_player = player;
            manager.AddComponentToEntity(player, test);

            ICollisionComponent collision = new CollisionComponent(true);
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

            ICollisionComponent collision = new CollisionComponent(true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            IScriptComponent movementStrategy = new FollowPlayerScript(gameTime, scene, player, scene.PlayerEntity, 4 * objectSize);

            manager.AddComponentToEntity(player, movementStrategy);
        }
    }
}
