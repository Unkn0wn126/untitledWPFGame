using Engine.Models.GameStateMachine;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Media;

namespace WPFGame
{
    public class GameEngine
    {
        private IGame _logicEngine;
        private MainWindow _graphicsEngine;
        System.Timers.Timer _logicTimer;
        private App _app;

        public GameEngine(int xRes, int yRes)
        {
            _logicEngine = new Game(xRes, yRes);

            _graphicsEngine = new MainWindow(_logicEngine, xRes, yRes);
            CompositionTarget.Rendering += _graphicsEngine.UpdateGraphics;

            // another timer to allow for independent non-graphics update
            _logicTimer = new System.Timers.Timer(16);
            _logicTimer.Elapsed += Update;
            _logicTimer.AutoReset = true;
            _logicTimer.Enabled = true;
        }

        public void StartRun()
        {
            _app = new App();

            _app.Run(_graphicsEngine);
            _logicEngine.State.CurrentState = GameState.RUNNING;
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            _logicEngine.Update();
        }

    }
}
