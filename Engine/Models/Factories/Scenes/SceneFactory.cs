using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.Components.Script;
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
    public class SceneFactory
    {
        public static IScene CreateBattleScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler)
        {
            int objectSize = 200;
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

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, currEntity, currTransform, grid);

            return scene;
        }

        public static IScene CreateScene(float xRes, float yRes, GameTime gameTime, GameInput gameInputHandler, bool generateTheGuy, int numOfObjectsOnX, int numOfObjectsOnY)
        {
            int objectSize = 1;
            int numOfObjectsInCell = 5;
            int cellSize = objectSize * numOfObjectsInCell;
            float baseCellXValue = (numOfObjectsOnX * objectSize) / (float)cellSize;
            float baseCellYValue = (numOfObjectsOnY * objectSize) / (float)cellSize;
            int numCellsX = (int)Math.Ceiling(baseCellXValue);
            int numCellsY = (int)Math.Ceiling(baseCellYValue);

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            // This is gonna be in a factory...
            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                for (int j = 0; j < numOfObjectsOnY; j++)
                {
                    ImgName currentName;
                    //currentName = i % 2 == 0 ? ImgNames.DIRT : ImgNames.COBBLESTONE;
                    currentName = ImgName.Dirt;

                    ITransformComponent currTransform = new TransformComponent(new Vector2(i * objectSize, j * objectSize), objectSize, objectSize, new Vector2(0, 0), 0);
                    IGraphicsComponent current = new GraphicsComponent(currentName);

                    uint currEntity = manager.AddEntity(currTransform);
                    manager.AddComponentToEntity(currEntity, current);
                }
            }

            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(i * objectSize, 0), objectSize, objectSize, new Vector2(0, 0), 1);
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(i + 1, false);
                manager.AddComponentToEntity(block, blockCollision);
            }

            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(i * objectSize, (numOfObjectsOnY - 1) * objectSize), objectSize, objectSize, new Vector2(0, 0), 1);
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(i + 1, false);
                manager.AddComponentToEntity(block, blockCollision);
            }

            for (int j = 1; j < numOfObjectsOnY; j++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(0, j * objectSize), objectSize, objectSize, new Vector2(0, 0), 1);
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(j + 1, false);
                manager.AddComponentToEntity(block, blockCollision);

            }

            for (int j = 1; j < numOfObjectsOnY; j++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2((numOfObjectsOnX - 1) * objectSize, j * objectSize), objectSize, objectSize, new Vector2(0, 0), 1);
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(j + 1, false);
                manager.AddComponentToEntity(block, blockCollision);

            }

            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize, objectSize), objectSize, objectSize, new Vector2(0, 0), 2);
            IGraphicsComponent test = new GraphicsComponent(ImgName.Player);
            //_playerTransform = playerTransform;

            uint player = manager.AddEntity(playerTransform);
            //_player = player;
            manager.AddComponentToEntity(player, test);

            ICollisionComponent collision = new CollisionComponent(3, true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, player, playerTransform, grid);



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

            ICollisionComponent collision = new CollisionComponent(3, true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            IScriptComponent movementStrategy = new FollowPlayerScript(gameTime, scene, player, scene.PlayerEntity, 4 * objectSize);

            manager.AddComponentToEntity(player, movementStrategy);
        }
    }
}
