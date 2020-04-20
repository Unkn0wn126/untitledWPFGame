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

        private ImagePaths _imgPaths;
        private IProcessor _graphicsProcessor;
        private List<IProcessor> _processors;
        private GameTime _gameTime;
        private GameInput _gameInputHandler;

        private float _xRes;
        private float _yRes;

        private Random _rnd;

        /*
         * does not need: imgPaths, xRes, yRes
         * may need: gameInputHandler, player context, gameTime from parameter
         * will need scene manager
         */
        public Game(ImagePaths imgPaths, GameInput gameInputHandler, float xRes, float yRes)
        {
            _xRes = xRes;
            _yRes = yRes;
            _gameTime = new GameTime();
            _gameInputHandler = gameInputHandler;
            _imgPaths = imgPaths;
            _processors = new List<IProcessor>();

            _rnd = new Random();

            State = new GameStateMachine
            {
                CurrentState = GameState.LOADING // prevent update of logic while not ready
            };

            ISceneFactory sceneFactory = new SceneFactory();
            int val = _rnd.Next(10, 20);
            CurrentScene = sceneFactory.CreateScene(xRes, yRes, _gameTime, _gameInputHandler, true, val, val);

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

            //secondsElapsed += _gameTime.DeltaTimeInSeconds;

            //if (State.IsLoading())
            //{
            //    loadingTime += _gameTime.DeltaTimeInSeconds;
            //}

            //if (loadingTime >= 3)
            //{
            //    State.CurrentState = GameState.RUNNING;
            //    secondsElapsed = 0;
            //    loadingTime = 0;
            //}
            
            //if (secondsElapsed >= 8 && State.IsRunning())
            //{
            //    int val = _rnd.Next(100, 200);
            //    State.CurrentState = GameState.LOADING;
            //    ISceneFactory sceneFactory = new SceneFactory();
            //    //CurrentScene = sceneFactory.CreateScene(_xRes, _yRes, _gameTime, _gameInputHandler, generateTheGuy, val, val);
            //    CurrentScene = sceneFactory.CreateBattleScene(_xRes, _yRes, _gameTime, _gameInputHandler);
            //    _graphicsProcessor.ChangeContext(CurrentScene);
            //    _processors.ForEach(x =>
            //    {
            //        x.ChangeContext(CurrentScene);
            //    });
            //    generateTheGuy = !generateTheGuy;
                
            //    secondsElapsed = 0;
            //}

            if (State.IsRunning())
            {
                CurrentScene.EntityManager.UpdateActiveEntities(CurrentScene.PlayerTransform);

                _processors.ForEach(x =>
                {
                    x.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
                });
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
