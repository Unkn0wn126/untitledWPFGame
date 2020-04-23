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

        private Random _rnd;


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

            _rnd = new Random();

            State = new GameStateMachine
            {
                CurrentState = GameState.LOADING // prevent update of logic while not ready
            };

            int val = _rnd.Next(10, 100);

            // Scene generation should take place elsewhere
            List<byte[]> metaScenes = new List<byte[]>();
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] current;
                var binaryFormatter = new BinaryFormatter();
                for (int i = 0; i < 10; i++)
                {
                    MetaScene metaScene = SceneFactory.CreateMetaScene(null, val, val, 1, 5);
                    binaryFormatter.Serialize(stream, metaScene);
                    current = stream.ToArray();
                    metaScenes.Add(current);
                    stream.SetLength(0);
                }
            }

            SceneManager = new SceneManager(metaScenes, _gameInputHandler, _gameTime);

            SceneManager.LoadNextScene();

            _graphicsProcessor = new GraphicsProcessor(SceneManager.CurrentScene);

            _processors.Add(new CollisionProcessor(SceneManager.CurrentScene));
            _processors.Add(new RigidBodyProcessor(SceneManager.CurrentScene));
            _processors.Add(new ScriptProcessor(SceneManager.CurrentScene));
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
    }
}
