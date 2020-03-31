using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.GameStateMachine
{
    public enum GameState
    {
        RUNNING,
        PAUSED,
        LOADING
    }
    public class GameStateMachine
    {
        public GameState CurrentState { get; set; }
        private GameState _lastState;

        public void Pause()
        {
            CurrentState = GameState.PAUSED;
        }

        public void UnPause()
        {
            CurrentState = GameState.RUNNING;
        }

        public void TogglePause()
        {
            if (CurrentState != GameState.PAUSED)
            {
                _lastState = CurrentState;
                CurrentState = GameState.PAUSED;
            }
            else
            {
                CurrentState = _lastState;
            }
        }

        public bool IsRunning()
        {
            return CurrentState != GameState.PAUSED;
        }
    }
}
