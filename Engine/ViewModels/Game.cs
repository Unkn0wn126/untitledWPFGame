using Engine.Models.Factories.Scenes;
using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using Engine.Processors;
using GameInputHandler;
using System.Collections.Generic;
using System.Diagnostics;
using TimeUtils;

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
        private bool _contextNeedsUpdate;

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
            SceneManager.BattleSceneMediator = new BattleSceneMediator();

            SceneManager.SceneChangeStarted += SetStateToLoading;
            SceneManager.SceneChangeFinished += InitializeContextUpdate;
        }

        /// <summary>
        /// Sets game state to loading
        /// </summary>
        private void SetStateToLoading()
        {
            State.CurrentState = GameState.Loading;
        }

        private void InitializeContextUpdate()
        {
            _contextNeedsUpdate = true;
        }

        /// <summary>
        /// Sets game state to running
        /// </summary>
        private void SetStateToRunning()
        {
            UpdateProcessorContext();
            _contextNeedsUpdate = false;

            State.CurrentState = 
                SceneManager.CurrentScene.SceneType == SceneType.General ? 
                GameState.Running : GameState.Battle;
        }

        /// <summary>
        /// Updates the processor context
        /// </summary>
        public void UpdateProcessorContext()
        {
            InitializeProcessors();
            _graphicsProcessor.ChangeContext(SceneManager.CurrentScene);

            foreach (IProcessor item in _processors)
            {
                item.ChangeContext(SceneManager.CurrentScene);
            }
        }

        /// <summary>
        /// Updates the game logic
        /// </summary>
        public void Update()
        {
            _gameTime.UpdateDeltaTime(); // keep track of elapsed time
            if (_contextNeedsUpdate)
            {
                SetStateToRunning();
            }
            if (State.IsRunning())
            {

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

        /// <summary>
        /// Initializes processors
        /// </summary>
        private void InitializeProcessors()
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

        /// <summary>
        /// Initializes a new game instance
        /// </summary>
        /// <param name="metaScenes"></param>
        public void InitializeGame(List<byte[]> metaScenes, int currentIndex)
        {
            SceneManager.UpdateScenes(metaScenes, currentIndex);
            UpdateProcessorContext();
        }
    }
}
