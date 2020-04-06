using Engine.Models.Components;
using Engine.Models.GameObjects;
using Engine.Models.Scenes;
using System.Collections.Generic;
using System.Numerics;

namespace Engine.ViewModels
{
    /// <summary>
    /// Container of all the game objects, scenes, etc.
    /// Keeps track of the context as a whole
    /// </summary>
    public class Game : IGame
    {
        private List<IScene> _scenes;
        private IGameComponent _playerMovement;
        private IGameObject _player;
        private List<IGameObject> _gameObjects;

        public IScene CurrentScene { get; set; }

        public List<IGraphicsComponent> GraphicsComponents { get; set; }

        public Game()
        {
            GraphicsComponents = new List<IGraphicsComponent>();

            _playerMovement = new PlayerMovementComponent();
            List<string> testList = new List<string> { "ground.png" };
            _gameObjects = new List<IGameObject>();
            IGraphicsComponent test = new GraphicsComponent(testList);
            GraphicsComponents.Add(test);
            _player = new LivingEntity(test, _playerMovement, 50, 50, new Vector2(0, 0), 10);
            _scenes = new List<IScene>();


            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    IGraphicsComponent current = new GraphicsComponent(new List<string> { "ground.png" });
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

            _scenes.Add(new GeneralScene(_gameObjects, _player));
            CurrentScene = _scenes[0];
        }

        public void HandleUserInput(MovementState newState)
        {
            ((PlayerMovementComponent)_playerMovement).SetMovementState(newState);
        }

        // Create objects here? Through factories...

        public void Update()
        {
            CurrentScene.Update();
        }
    }
}
