﻿using Engine.Models.GameStateMachine;
using Engine.ViewModels;
using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace WPFGame
{
    public class GameEngine
    {
        private IGame _logicEngine;
        private MainWindow _graphicsEngine;
        private App _app;
        private bool _isRunning = false;
        private Thread _logicThread;

        public GameEngine(int xRes, int yRes)
        {
            GameInput gameInputHandler = new GameInput();
            _logicEngine = new Game(gameInputHandler, xRes, yRes);

            _graphicsEngine = new MainWindow(gameInputHandler, _logicEngine, xRes, yRes);
            CompositionTarget.Rendering += _graphicsEngine.UpdateGraphics;
            _logicThread = new Thread(Update);
        }

        public void StartRun()
        {
            _app = new App();
            _isRunning = true;

            _logicThread.Start();
            _app.Run(_graphicsEngine);

            _logicEngine.State.CurrentState = GameState.RUNNING;
            _isRunning = false;
        }

        private void Update()
        {
            while (_isRunning)
            {
                    _logicEngine.Update();
            }

        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            _logicEngine.Update();
        }

    }
}
