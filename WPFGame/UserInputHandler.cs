using GameInputHandler;
using System.Collections.Generic;
using System.Windows.Input;

namespace WPFGame
{
    /// <summary>
    /// Helper class to transfer
    /// graphics key events
    /// to logic for further
    /// processes
    /// </summary>
    public class UserInputHandler
    {
        private readonly Dictionary<Key, GameKey> _keyCodes =
            new Dictionary<Key, GameKey>();

        private GameInput _gameInputHandler;

        public UserInputHandler(GameInput gameInputHandler, Configuration gameConfig)
        {
            _gameInputHandler = gameInputHandler;
            InitializeKeyCodes(gameConfig);
        }

        /// <summary>
        /// Updates the current key
        /// dictionary based on the new
        /// configuration
        /// </summary>
        /// <param name="gameConfig"></param>
        public void UpdateConfiguration(Configuration gameConfig)
        {
            _keyCodes.Clear();
            InitializeKeyCodes(gameConfig);
        }

        /// <summary>
        /// Initializes the keys dictionary
        /// </summary>
        /// <param name="gameConfig"></param>
        private void InitializeKeyCodes(Configuration gameConfig)
        {
            // Kind of like a state machine of pressed down keys
            // powers of 2 to allow diagonal movement
            _keyCodes.Add(gameConfig.Up, GameKey.Up);
            _keyCodes.Add(gameConfig.Left, GameKey.Left);
            _keyCodes.Add(gameConfig.Down, GameKey.Down);
            _keyCodes.Add(gameConfig.Right, GameKey.Right);
            _keyCodes.Add(gameConfig.Escape, GameKey.Escape);
            _keyCodes.Add(gameConfig.Action, GameKey.Action);
            _keyCodes.Add(gameConfig.DetectiveMode, GameKey.DetectiveMode);
            _keyCodes.Add(gameConfig.Back, GameKey.Back);
            _keyCodes.Add(gameConfig.Space, GameKey.Space);
        }

        /// <summary>
        /// Handles the key pressed
        /// event
        /// </summary>
        /// <param name="e"></param>
        public void HandleKeyPressed(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue |= _keyCodes[e];
            }
        }

        /// <summary>
        /// Handles the key released
        /// event
        /// </summary>
        /// <param name="e"></param>
        public void HandleKeyReleased(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue &= ~_keyCodes[e];
            }
        }
    }
}
