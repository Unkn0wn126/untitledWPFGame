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
using System.Diagnostics;

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

            SceneManager.SceneChangeStarted += SetStateToLoading;
            SceneManager.SceneChangeFinished += SetStateToRunning;
        }

        private void SetStateToLoading()
        {
            State.CurrentState = GameState.Loading;
        }

        private void SetStateToRunning()
        {
            UpdateProcessorContext();
            State.CurrentState = GameState.Running;
        }

        public void UpdateProcessorContext()
        {
            CheckProcessorInitialization();
            _graphicsProcessor.ChangeContext(SceneManager.CurrentScene);

            _processors.ForEach(x =>
            {
                x.ChangeContext(SceneManager.CurrentScene);
            });
        }

        private float timeElapsed = 0;

        /// <summary>
        /// Updates the game logic
        /// </summary>
        public void Update()
        {
            _gameTime.UpdateDeltaTime(); // keep track of elapsed time

            if (State.IsRunning())
            {
                //timeElapsed += _gameTime.DeltaTimeInSeconds;

                //if (timeElapsed >= 8)
                //{
                //    SceneManager.LoadNextScene();
                //    timeElapsed = 0;
                //}

                SceneManager.CurrentScene.EntityManager.UpdateActiveEntities(SceneManager.CurrentScene.SceneCamera.FocusPoint);

                foreach (var x in _processors)
                {
                    if (State.IsRunning())
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

        private void CheckProcessorInitialization()
        {
            if (_graphicsProcessor == null)
            {
                _graphicsProcessor = new GraphicsProcessor(SceneManager.CurrentScene);
            }
            if (_processors.Count == 0)
            {
                _processors.Add(new CollisionProcessor(SceneManager.CurrentScene));
                _processors.Add(new RigidBodyProcessor(SceneManager.CurrentScene));
                _processors.Add(new ScriptProcessor(SceneManager.CurrentScene));
            }
        }

        public void InitializeGame(List<byte[]> metaScenes)
        {
            SceneManager.UpdateScenes(metaScenes);

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
