using Engine.ViewModels;
using GameInputHandler;
using ResourceManagers.Images;
using System.Threading.Tasks;
using TimeUtils;

namespace WPFGame
{
    /// <summary>
    /// Holds the graphics
    /// and logic context
    /// of the game
    /// </summary>
    public class GameEngine
    {
        private readonly IGame _logicEngine;

        private readonly MainWindow _graphicsEngine;
        private readonly Task _logicThread;

        private readonly GameTime _gameTime;
        private readonly GameInput _gameInputHandler;
        private readonly ImagePaths _imagePaths;

        private App _app;

        private bool _isRunning = false;

        public GameEngine()
        {
            _gameInputHandler = new GameInput();
            _gameTime = new GameTime();
            _imagePaths = new ImagePaths();
            _logicEngine = new Game(_gameInputHandler, _gameTime);

            _graphicsEngine = new MainWindow(_imagePaths, _gameInputHandler, _logicEngine);
            _logicThread = new Task(Update);
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartRun()
        {
            _app = new App();
            _isRunning = true;

            _logicThread.Start();

            _app.Run(_graphicsEngine);

            _isRunning = false;
        }

        /// <summary>
        /// Updates the game logic
        /// </summary>
        private void Update()
        {
            while (_isRunning)
            {
                    _logicEngine.Update();
            }

        }

    }
}
