using Engine.Models.GameStateMachine;
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private DateTime _time1 = DateTime.Now;
        private DateTime _time2 = DateTime.Now;

        public GameEngine(int xRes, int yRes)
        {
            _logicEngine = new Game(xRes, yRes);

            _graphicsEngine = new MainWindow(_logicEngine, xRes, yRes);
            CompositionTarget.Rendering += _graphicsEngine.UpdateGraphics;

            // another timer to allow for independent non-graphics update
            //_logicTimer = new System.Timers.Timer(16);
            //_logicTimer.Elapsed += Update;
            //_logicTimer.AutoReset = true;
            //_logicTimer.Enabled = true;
        }

        public void StartRun()
        {
            _app = new App();

            Thread t = new Thread(Update);
            isRunning = true;
            t.Start();
            _app.Run(_graphicsEngine);
            _logicEngine.State.CurrentState = GameState.RUNNING;
            isRunning = false;
        }

        private float timer = 0;
        private float timeout = 1/60;
        bool isRunning = false;

        private void Update()
        {
            while (isRunning)
            {
                _time2 = DateTime.Now;
                float deltaTime = (_time2.Ticks - _time1.Ticks) / 10000000f; // in seconds (one tick = 1 / 10 000 000 of a second
                _time1 = _time2;

                timer += deltaTime;

                //Trace.WriteLine($"Timer: {timer}");

                if (timer >= timeout)
                {
                    timer = 0;
                    _logicEngine.Update();
                }
            }

        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            _logicEngine.Update();
        }

    }
}
