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
using System.Threading.Tasks;
using Engine.Models.Factories;
using Engine.Models.Factories.Scenes;

namespace Engine.ViewModels
{
    /// <summary>
    /// Container of the current game context
    /// Keeps track of the context as a whole
    /// </summary>
    public class Game : IGame
    {
        public IScene CurrentScene { get; set; }
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

            int val = _rnd.Next(1000, 2000);

            // Scene generation should take place elsewhere
            List<MetaScene> metaScenes = new List<MetaScene>();
            for (int i = 0; i < 10; i++)
            {
                metaScenes.Add(SceneFactory.CreateMetaScene(val, val, 1, 5));
            }
            for (int i = 0; i < metaScenes.Count - 1; i++)
            {
                metaScenes[i].NextScene = metaScenes[i + 1].ID;
            }

            SceneManager = new SceneManager(metaScenes, _gameInputHandler, _gameTime);
            
            //CurrentScene = SceneFactory.CreateScene(xRes, yRes, _gameTime, _gameInputHandler, true, val, val);
            CurrentScene = SceneManager.LoadNextScene();

            _graphicsProcessor = new GraphicsProcessor(CurrentScene);

            _processors.Add(new CollisionProcessor(CurrentScene));
            _processors.Add(new RigidBodyProcessor(CurrentScene));
            _processors.Add(new ScriptProcessor(CurrentScene));
        }

        private float secondsElapsed = 0;
        private float loadingTime = 0;
        private bool generateTheGuy = false;
        /// <summary>
        /// Updates the game logic
        /// </summary>
        public void Update()
        {
            _gameTime.UpdateDeltaTime(); // keep track of elapsed time

            if (State.IsRunning())
            {
                secondsElapsed += _gameTime.DeltaTimeInSeconds;
            }

            if (State.IsLoading())
            {
                loadingTime += _gameTime.DeltaTimeInSeconds;
            }

            if (loadingTime >= 3)
            {
                State.CurrentState = GameState.RUNNING;
                secondsElapsed = 0;
                loadingTime = 0;
            }

            if (secondsElapsed >= 60 && State.IsRunning())
            {
                int val = _rnd.Next(100, 200);
                State.CurrentState = GameState.LOADING;
                //CurrentScene = SceneFactory.CreateScene(_xRes, _yRes, _gameTime, _gameInputHandler, generateTheGuy, val, val);
                //CurrentScene = SceneFactory.CreateBattleScene(_xRes, _yRes, _gameTime, _gameInputHandler);
                CurrentScene = SceneManager.LoadNextScene();
                _graphicsProcessor.ChangeContext(CurrentScene);
                _processors.ForEach(x =>
                {
                    x.ChangeContext(CurrentScene);
                });
                generateTheGuy = !generateTheGuy;

                secondsElapsed = 0;
            }

            if (State.IsRunning())
            {
                CurrentScene.EntityManager.UpdateActiveEntities(CurrentScene.PlayerTransform);

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
