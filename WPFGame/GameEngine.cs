using Engine.Models.GameStateMachine;
using Engine.ViewModels;
using GameInputHandler;
using ResourceManagers.Images;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TimeUtils;

namespace WPFGame
{
    public class GameEngine
    {
        private IGame _logicEngine;

        private MainWindow _graphicsEngine;
        private App _app;

        private bool _isRunning = false;
        private Task _logicThread;

        private GameTime _gameTime;
        private GameInput _gameInputHandler;
        private ImagePaths _imagePaths;

        public GameEngine()
        {
            _gameInputHandler = new GameInput();
            _gameTime = new GameTime();
            _imagePaths = new ImagePaths();
            _logicEngine = new Game(_gameInputHandler, _gameTime);

            _graphicsEngine = new MainWindow(_imagePaths, _gameInputHandler, _logicEngine);
            //CompositionTarget.Rendering += _graphicsEngine.UpdateGraphics;
            _logicThread = new Task(Update);
        }

        public void StartRun()
        {
            _app = new App();
            _isRunning = true;

            _logicThread.Start();

            _app.Run(_graphicsEngine);

            _isRunning = false;
        }

        private void Update()
        {
            while (_isRunning)
            {
                    _logicEngine.Update();
            }

        }

    }
}
