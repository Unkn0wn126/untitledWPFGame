using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using Engine.Processors;
using ResourceManagers.Images;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using TimeUtils;
using Engine.Models.Components.Script;

namespace Engine.ViewModels
{
    /// <summary>
    /// Container of all the game objects, scenes, etc.
    /// Keeps track of the context as a whole
    /// </summary>
    public class Game : IGame
    {
        private List<IScene> _scenes;

        public IScene CurrentScene { get; set; }

        public List<IGraphicsComponent> GraphicsComponents { get; set; }
        public GameStateMachine State { get; set; }
        private ImagePaths _imgPaths;
        private IProcessor _graphicsProcessor;
        private IProcessor _collisionProcessor;
        private IProcessor _rigidBodyProcessor;
        private ITransformComponent _playerTransform;

        private GameInput _gameInputHandler;

        private PlayerMovementScript _movementStrategy;

        private IScene GenerateScene(float xRes, float yRes)
        {
            int objectSize = 50;
            int numOfObjectsInCell = 5;
            int cellSize = objectSize * numOfObjectsInCell;
            int numOfObjectsOnX = 10;
            int numOfObjectsOnY = 10;
            int numCellsX = (numOfObjectsOnX * objectSize) / cellSize;
            int numCellsY = (numOfObjectsOnY * objectSize) / cellSize;

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

                    ITransformComponent currTransform = new TransformComponent(new Vector2(i * objectSize, j * objectSize), objectSize, objectSize, new Vector2(0, 0));
                    IGraphicsComponent current = new GraphicsComponent(currentName);

                    uint currEntity = manager.AddEntity(currTransform);
                    manager.AddComponentToEntity(currEntity, current);
                }
            }

            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(i * objectSize, 0), objectSize, objectSize, new Vector2(0, 0));
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(i + 1, false);
                manager.AddComponentToEntity(block, blockCollision);
            }

            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(i * objectSize, (numOfObjectsOnY - 1) * objectSize), objectSize, objectSize, new Vector2(0, 0));
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(i + 1, false);
                manager.AddComponentToEntity(block, blockCollision);
            }

            for (int j = 1; j < numOfObjectsOnY; j ++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2(0, j * objectSize), objectSize, objectSize, new Vector2(0, 0));
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(j + 1, false);
                manager.AddComponentToEntity(block, blockCollision);

            }

            for (int j = 1; j < numOfObjectsOnY; j ++)
            {
                ITransformComponent blockTransform = new TransformComponent(new Vector2((numOfObjectsOnX - 1) * objectSize, j * objectSize), objectSize, objectSize, new Vector2(0, 0));
                IGraphicsComponent blockcurrent = new GraphicsComponent(ImgName.Rock);

                uint block = manager.AddEntity(blockTransform);
                manager.AddComponentToEntity(block, blockcurrent);

                ICollisionComponent blockCollision = new CollisionComponent(j + 1, false);
                manager.AddComponentToEntity(block, blockCollision);

            }

            ITransformComponent playerTransform = new TransformComponent(new Vector2(objectSize, objectSize), objectSize, objectSize, new Vector2(0, 0));
            IGraphicsComponent test = new GraphicsComponent(ImgName.Player);
            _playerTransform = playerTransform;

            uint player = manager.AddEntity(playerTransform);
            manager.AddComponentToEntity(player, test);

            ICollisionComponent collision = new CollisionComponent(3, true);
            manager.AddComponentToEntity(player, collision);

            IRigidBodyComponent rigidBody = new RigidBodyComponent();
            manager.AddComponentToEntity(player, rigidBody);

            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, player, playerTransform, grid);

            _graphicsProcessor = new GraphicsProcessor(scene, playerTransform);
            _collisionProcessor = new CollisionProcessor(scene);
            _rigidBodyProcessor = new RigidBodyProcessor(scene);

            _movementStrategy = new PlayerMovementScript(_gameTime, _gameInputHandler, scene, player, 50f);

            return scene;
        }

        public Game(ImagePaths imgPaths, GameInput gameInputHandler, float xRes, float yRes)
        {
            _gameTime = new GameTime();
            _gameInputHandler = gameInputHandler;
            GraphicsComponents = new List<IGraphicsComponent>();
            _imgPaths = imgPaths;
            _scenes = new List<IScene>();

            State = new GameStateMachine
            {
                CurrentState = GameState.LOADING
            };

            _scenes.Add(GenerateScene(xRes, yRes));
            CurrentScene = _scenes[0];
        }

        private GameTime _gameTime;
        //private float milliSecondsPassed = 0;
        // Create objects here? Through factories...
        public void Update()
        {
            _gameTime.UpdateDeltaTime();
            if (State.IsRunning())
            {
                CurrentScene.EntityManager.UpdateActiveEntities(_playerTransform);
                _collisionProcessor.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
                _movementStrategy.Update();

                _rigidBodyProcessor.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
            }
        }

        public void UpdateGraphics()
        {
            _graphicsProcessor?.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
        }
    }
}
