using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models.GameStateMachine
{
    public enum GameState
    {
        Running,
        Paused,
        Menu,
        Loading
    }
    public class GameStateMachine
    {
        public GameState CurrentState { get; set; }
        private GameState _lastState;

        public void Pause()
        {
            CurrentState = GameState.Paused;
        }

        public void UnPause()
        {
            CurrentState = GameState.Running;
        }

        public void TogglePause()
        {
            if (CurrentState != GameState.Paused)
            {
                _lastState = CurrentState;
                CurrentState = GameState.Paused;
            }
            else
            {
                CurrentState = _lastState;
            }
        }

        public bool IsRunning()
        {
            return CurrentState == GameState.Running;
        }

        public bool IsPaused()
        {
            return CurrentState == GameState.Paused;
        }

        public bool IsLoading()
        {
            return CurrentState == GameState.Loading;
        }
    }
}
