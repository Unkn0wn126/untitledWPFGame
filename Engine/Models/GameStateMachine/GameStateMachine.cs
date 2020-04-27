namespace Engine.Models.GameStateMachine
{
    /// <summary>
    /// Used for determining
    /// the state of the game
    /// </summary>
    public enum GameState
    {
        Running,
        Paused,
        Menu,
        Battle,
        Loading
    }

    /// <summary>
    /// Holds the current state
    /// of the game.
    /// Helps to prevent
    /// unnecessary updates,
    /// crashes, etc.
    /// </summary>
    public class GameStateMachine
    {
        public GameState CurrentState { get; set; }
        private GameState _lastState;

        /// <summary>
        /// Sets the state to Paused
        /// </summary>
        public void Pause()
        {
            CurrentState = GameState.Paused;
        }

        /// <summary>
        /// Sets the state to Running
        /// </summary>
        public void UnPause()
        {
            CurrentState = GameState.Running;
        }

        /// <summary>
        /// Sets the state from Paused
        /// to the previous state
        /// </summary>
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

        /// <summary>
        /// Checks if the state is Running
        /// </summary>
        /// <returns></returns>
        public bool IsRunning()
        {
            return CurrentState == GameState.Running || CurrentState == GameState.Battle;
        }

        /// <summary>
        /// Checks if the state is Paused
        /// </summary>
        /// <returns></returns>
        public bool IsPaused()
        {
            return CurrentState == GameState.Paused;
        }

        /// <summary>
        /// Checks if the state is Loading
        /// </summary>
        /// <returns></returns>
        public bool IsLoading()
        {
            return CurrentState == GameState.Loading;
        }
    }
}
