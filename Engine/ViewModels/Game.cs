using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using Engine.Processors;
using GameInputHandler;
using System;
using System.Collections.Generic;
using TimeUtils;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.Models.Components;

namespace Engine.ViewModels
{
    /// <summary>
    /// Container of the current game context
    /// Keeps track of the context as a whole
    /// </summary>
    public class Game : IGame
    {
        public GameStateMachine State { get; set; }

        private IProcessor _graphicsProcessor;
        private List<IProcessor> _processors;

        private GameTime _gameTime;
        private GameInput _gameInputHandler;
        public ISceneManager SceneManager { get; set; }


        /*
         * does not need: imgPaths, xRes, yRes
         * may need: gameInputHandler, gameTime from parameter
         * will need scene manager
         */
        public Game(GameInput gameInputHandler, GameTime gameTime)
        {
            _gameTime = gameTime;
            _gameInputHandler = gameInputHandler;
            _processors = new List<IProcessor>();

            State = new GameStateMachine
            {
                CurrentState = GameState.Loading // prevent update of logic while not ready
            };

            SceneManager = new SceneManager(_gameInputHandler, _gameTime);
        }

        public void UpdateProcessorContext()
        {
            _graphicsProcessor.ChangeContext(SceneManager.CurrentScene);

            _processors.ForEach(x =>
            {
                x.ChangeContext(SceneManager.CurrentScene);
            });
        }
        /// <summary>
        /// Updates the game logic
        /// </summary>
        public void Update()
        {
            _gameTime.UpdateDeltaTime(); // keep track of elapsed time

            if (State.IsRunning())
            {
                SceneManager.CurrentScene.EntityManager.UpdateActiveEntities(SceneManager.CurrentScene.SceneCamera.FocusPoint);

                foreach (var x in _processors)
                {
                    x.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
                }
            }
        }

        /// <summary>
        /// Updates which entities should be visible
        /// </summary>
        public void UpdateGraphics()
        {
            _graphicsProcessor?.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
        }

        public void InitializeGame(List<byte[]> metaScenes)
        {
            SceneManager.UpdateScenes(metaScenes);

            SceneManager.LoadNextScene();

            if (_graphicsProcessor == null)
            {
                _graphicsProcessor = new GraphicsProcessor(SceneManager.CurrentScene);
            }
            else
            {
                _graphicsProcessor.ChangeContext(SceneManager.CurrentScene);
            }

            if (_processors.Count == 0)
            {
                _processors.Add(new CollisionProcessor(SceneManager.CurrentScene));
                _processors.Add(new RigidBodyProcessor(SceneManager.CurrentScene));
                _processors.Add(new ScriptProcessor(SceneManager.CurrentScene));
            }
            else
            {
                UpdateProcessorContext();
            }


        }
    }
}
