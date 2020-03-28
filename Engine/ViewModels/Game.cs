using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
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
        private GameStateMachine _state;
        private ImagePaths _imgPaths;

        private List<IScene> _scenes;
        private IScene _currentScene;

        private IGameComponent _playerMovement;
        private IGameObject _player;

        private List<IGraphicsComponent> _graphicsComponents;
        private List<IGameObject> _gameObjects;

        public IScene CurrentScene { get => _currentScene; set => _currentScene = value; }


        public List<IGraphicsComponent> GraphicsComponents { get => _graphicsComponents; set => _graphicsComponents = value; }
        public GameStateMachine State { get => _state; set => _state = value; }
        public ImagePaths ImgPaths { get => _imgPaths; set => _imgPaths = value; }

        public Game(float xRes, float yRes)
        {
            GraphicsComponents = new List<IGraphicsComponent>();
            _imgPaths = new ImagePaths();

            _state = new GameStateMachine
            {
                CurrentState = GameState.RUNNING
            };

            _playerMovement = new PlayerMovementComponent();
            List<ImgNames> testList = new List<ImgNames> { ImgNames.PLAYER };
            _gameObjects = new List<IGameObject>();
            IGraphicsComponent test = new GraphicsComponent(testList);
            GraphicsComponents.Add(test);
            _player = new LivingEntity(test, _playerMovement, 50, 50, new Vector2(0, 0), 10);
            _scenes = new List<IScene>();

            // This is gonna be in a factory...
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    ImgNames currentName;
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        currentName = ImgNames.DIRT;
                    }
                    else
                    {
                        currentName = ImgNames.COBBLESTONE;
                    }
                    IGraphicsComponent current = new GraphicsComponent(new List<ImgNames> { currentName });
                    current.Width = 50;
                    current.Height = 50;
                    Vector2 currentPos = current.Position;
                    currentPos.X = i * 50;
                    currentPos.Y = j * 50;
                    current.Position = currentPos;
                    GraphicsComponents.Add(current);

                    _gameObjects.Add(new Ground(current, new Vector2(i * 50, j * 50), 50, 50));
                }
            }
            _gameObjects.Add(_player);

            _scenes.Add(new GeneralScene(_gameObjects, _player, xRes, yRes));
            _currentScene = _scenes[0];
        }

        public void HandleUserInput(MovementState newState)
        {
            ((PlayerMovementComponent)_playerMovement).SetMovementState(newState);
        }

        // Create objects here? Through factories...

        public void Update()
        {
            if (_state.IsRunning())
            {
                _currentScene.Update();
            }
        }
    }
}
