using Engine.Coordinates;
using Engine.EntityManagers;
using Engine.Models.Cameras;
using Engine.Models.Components;
using Engine.Models.GameStateMachine;
using Engine.Models.MovementStateStrategies;
using Engine.Models.Scenes;
using Engine.Processors;
using Engine.ResourceConstants.Images;
using System;
using System.Collections.Generic;
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
        private ITransformComponent _playerTransform;

        private IMovementStrategy _movementStrategy;

        private IScene GenerateScene(float xRes, float yRes)
        {
            int objectSize = 50;
            int numOfObjectsInCell = 5;
            int cellSize = objectSize * numOfObjectsInCell;
            int numOfObjectsOnX = 100;
            int numOfObjectsOnY = 100;
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

            //_playerMovement = new EntityMovementComponent();
            //List<ImgNames> testList = new List<ImgNames> { ImgNames.PLAYER };
            //_gameObjects = new List<IGameObject>();
            ITransformComponent playerTransform = new TransformComponent(new Vector2(0, 0), objectSize, objectSize, new Vector2(0, 0));
            IGraphicsComponent test = new GraphicsComponent(ImgNames.PLAYER);
            _playerTransform = playerTransform;
            //GraphicsComponents.Add(test);
            //_player = new LivingEntity(grid, test, _playerMovement, playerTransform, new GeneralEntityStats(100, 5f, 10, 10));

            uint player = manager.AddEntity(playerTransform);
            manager.AddComponentToEntity(player, test);
            IScene scene = new GeneralScene(new Camera(xRes, yRes), manager, player, playerTransform, grid);

            _graphicsProcessor = new GraphicsProcessor(scene, playerTransform);

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

            State.CurrentState = GameState.RUNNING;
        }

        public void HandleUserInput(IMovementStrategy newState)
        {
            _movementStrategy = newState;
        }

        // Create objects here? Through factories...

        public void Update()
        {
            // TODO: Change this to a state class resolved system
            if (State.IsRunning())
            {
                _movementStrategy?.ExecuteStrategy(CurrentScene.PlayerEntity, CurrentScene.Transform, CurrentScene.Coordinates);
            }
        }

        public void UpdateGraphics()
        {
            _graphicsProcessor?.ProcessOnEeGameTick(0);
        }
    }
}
