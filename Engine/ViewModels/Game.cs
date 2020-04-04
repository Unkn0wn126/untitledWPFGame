using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.Components.Collision;
using Engine.Models.Components.RigidBody;
using Engine.Models.GameStateMachine;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.Processors;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

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
        public ImagePaths ImgPaths { get; set; }
        private IProcessor _graphicsProcessor;
        private IProcessor _collisionProcessor;
        private IProcessor _rigidBodyProcessor;
        private ITransformComponent _playerTransform;

        private PlayerMovementScript _movementStrategy;

        private IScene GenerateScene(float xRes, float yRes)
        {
            int objectSize = 50;
            int numOfObjectsInCell = 5;
            int cellSize = objectSize * numOfObjectsInCell;
            int numOfObjectsOnX = 1000;
            int numOfObjectsOnY = 1000;
            int numCellsX = (numOfObjectsOnX * objectSize) / cellSize;
            int numCellsY = (numOfObjectsOnY * objectSize) / cellSize;

            ISpatialIndex grid = new Grid(numCellsX, numCellsY, cellSize);

            IEntityManager manager = new EntityManager(grid);

            // This is gonna be in a factory...
            for (int i = 0; i < numOfObjectsOnX; i++)
            {
                for (int j = 0; j < numOfObjectsOnY; j++)
                {
                    ImgNames currentName;
                    currentName = i % 2 == 0 ? ImgNames.DIRT : ImgNames.COBBLESTONE;

                    ITransformComponent currTransform = new TransformComponent(new Vector2(i * objectSize, j * objectSize), objectSize, objectSize, new Vector2(0, 0));
                    IGraphicsComponent current = new GraphicsComponent(currentName);

                    uint currEntity = manager.AddEntity(currTransform);
                    manager.AddComponentToEntity(currEntity, current);
                }
            }

            for (int i = 1; i < numOfObjectsOnX / 2 - 1; i+=2)
            {
                for (int j = 0; j < numOfObjectsOnY / 2; j+=2)
                {
                    ITransformComponent blockTransform = new TransformComponent(new Vector2(i * objectSize * 2, j * objectSize * 2), objectSize, objectSize, new Vector2(0, 0));
                    IGraphicsComponent blockcurrent = new GraphicsComponent(ImgNames.ROCK);

                    uint block = manager.AddEntity(blockTransform);
                    manager.AddComponentToEntity(block, blockcurrent);

                    ICollisionComponent blockCollision = new CollisionComponent(i + 1, false);
                    manager.AddComponentToEntity(block, blockCollision);
                }
            }

            ITransformComponent playerTransform = new TransformComponent(new Vector2(0, 0), objectSize, objectSize, new Vector2(0, 0));
            IGraphicsComponent test = new GraphicsComponent(ImgNames.PLAYER);
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

            _movementStrategy = new PlayerMovementScript(scene, player, 5f);

            return scene;
        }

        public Game(float xRes, float yRes)
        {
            GraphicsComponents = new List<IGraphicsComponent>();
            ImgPaths = new ImagePaths();
            _scenes = new List<IScene>();

            State = new GameStateMachine
            {
                CurrentState = GameState.LOADING
            };

            _scenes.Add(GenerateScene(xRes, yRes));
            CurrentScene = _scenes[0];

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            //State.CurrentState = GameState.RUNNING;
        }

        public void HandleUserInput(AxisStrategy axisStrategy)
        {
            _movementStrategy.UpdatePosition(axisStrategy);
        }

        private long _lastFrame = 0;
        private Stopwatch _stopwatch;
        // Create objects here? Through factories...
        public void Update()
        {
            // TODO: Change this to a state class resolved system
            if (State.IsRunning())
            {
                Trace.WriteLine($"Elapsed: {_stopwatch.ElapsedMilliseconds}; Difference: {_stopwatch.ElapsedMilliseconds - _lastFrame}");
                long diff = _stopwatch.ElapsedMilliseconds - _lastFrame;
                _lastFrame = _stopwatch.ElapsedMilliseconds;
                _movementStrategy.ApplyForce();
                CurrentScene.EntityManager.UpdateActiveEntities(_playerTransform);
                _collisionProcessor.ProcessOnEeGameTick(diff);
                _rigidBodyProcessor.ProcessOnEeGameTick(diff);
            }
        }

        public void UpdateGraphics()
        {
            _graphicsProcessor?.ProcessOnEeGameTick(0);
        }
    }
}
