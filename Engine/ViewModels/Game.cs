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

        /*
         * does not need: imgPaths, xRes, yRes
         * may need: gameInputHandler, player context
         * will need scene manager
         */
        public Game(ImagePaths imgPaths, GameInput gameInputHandler, float xRes, float yRes)
        {
            _gameTime = new GameTime();
            _gameInputHandler = gameInputHandler;
            _imgPaths = imgPaths;
            _processors = new List<IProcessor>();

            State = new GameStateMachine
            {
                CurrentState = GameState.LOADING
            };

            ISceneFactory sceneFactory = new SceneFactory();
            CurrentScene = sceneFactory.CreateScene(xRes, yRes, _gameTime, gameInputHandler);

            _graphicsProcessor = new GraphicsProcessor(CurrentScene);

            _processors.Add(new CollisionProcessor(CurrentScene));
            _processors.Add(new RigidBodyProcessor(CurrentScene));
            _processors.Add(new ScriptProcessor(CurrentScene));
        }


        public void Update()
        {
            _gameTime.UpdateDeltaTime();
            if (State.IsRunning())
            {
                CurrentScene.EntityManager.UpdateActiveEntities(CurrentScene.PlayerTransform);

                _processors.ForEach(x =>
                {
                    x.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
                });
            }
        }

        public void UpdateGraphics()
        {
            _graphicsProcessor?.ProcessOneGameTick(_gameTime.DeltaTimeInMilliseconds);
        }
    }
}
