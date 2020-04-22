using GameInputHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace WPFGame
{
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

        public void UpdateConfiguration(Configuration gameConfig)
        {
            _keyCodes.Clear();
            InitializeKeyCodes(gameConfig);
        }

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
            _keyCodes.Add(gameConfig.Action2, GameKey.Action2);
            _keyCodes.Add(gameConfig.Back, GameKey.Back);
            _keyCodes.Add(gameConfig.Space, GameKey.Space);
        }

        public void HandleKeyPressed(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue |= _keyCodes[e];
                //Trace.WriteLine($"Pressing, value is: {_gameInputHandler.CurrentKeyValue}");
            }
        }

        public void HandleKeyReleased(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue &= ~_keyCodes[e];
                //Trace.WriteLine($"Releasing, value is: {_gameInputHandler.CurrentKeyValue}");
            }
        }
    }
}
