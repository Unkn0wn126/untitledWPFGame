using Engine.Models.Factories.Scenes;
using Engine.Models.GameStateMachine;
using Engine.Models.Scenes;
using Engine.Processors;
using GameInputHandler;
using System.Collections.Generic;
using TimeUtils;

namespace Engine.ViewModels
{

    public class Game : IGame
    {
        private readonly List<IProcessor> _processors;
        private readonly GameTime _gameTime;
        private readonly GameInput _gameInputHandler;

        private bool _contextNeedsUpdate;
        private IProcessor _graphicsProcessor;

        public GameStateMachine State { get; set; }
        public ISceneManager SceneManager { get; set; }

        public Game(GameInput gameInputHandler, GameTime gameTime)
        {
            _gameTime = gameTime;
            _gameInputHandler = gameInputHandler;
            _processors = new List<IProcessor>();

            State = new GameStateMachine
            {
                CurrentState = GameState.Loading // prevent update of logic while not ready
            };

            SceneManager = new SceneManager(_gameInputHandler, _gameTime)
            {
                BattleSceneMediator = new BattleSceneMediator()
            };

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

        /// <summary>
        /// Saves the information
        /// that the context should update.
        /// Used to not update context
        /// directly via a delegate
        /// to prevent several crashes.
        /// </summary>
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

        public void UpdateProcessorContext()
        {
            InitializeProcessors();
            _graphicsProcessor.ChangeContext(SceneManager.CurrentScene);

            foreach (IProcessor item in _processors)
            {
                item.ChangeContext(SceneManager.CurrentScene);
            }
        }

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

        public void InitializeGame(List<byte[]> metaScenes, int currentIndex)
        {
            SceneManager.UpdateScenes(metaScenes, currentIndex);
            UpdateProcessorContext();
        }
    }
}
