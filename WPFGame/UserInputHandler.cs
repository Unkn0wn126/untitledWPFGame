using Engine.Models.MovementStateStrategies;
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

        public UserInputHandler(GameInput gameInputHandler)
        {
            _gameInputHandler = gameInputHandler;
            InitializeUserInputActions();
        }

        private void InitializeUserInputActions()
        {
            InitializeKeyCodes();
        }

        private void InitializeKeyCodes()
        {
            // Kind of like a state machine of pressed down keys
            // powers of 2 to allow diagonal movement
            _keyCodes.Add(Key.W, GameKey.Up);
            _keyCodes.Add(Key.A, GameKey.Left);
            _keyCodes.Add(Key.S, GameKey.Down);
            _keyCodes.Add(Key.D, GameKey.Right);
            _keyCodes.Add(Key.Escape, GameKey.Escape);
            _keyCodes.Add(Key.E, GameKey.Action);
            _keyCodes.Add(Key.Q, GameKey.Action2);
            _keyCodes.Add(Key.B, GameKey.Back);
            _keyCodes.Add(Key.Space, GameKey.Space);
        }

        public void HandleKeyPressed(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue |= _keyCodes[e];
                Trace.WriteLine($"Pressing, value is: {_gameInputHandler.CurrentKeyValue}");
            }
        }

        public void HandleKeyReleased(Key e)
        {
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue &= ~_keyCodes[e];
                Trace.WriteLine($"Releasing, value is: {_gameInputHandler.CurrentKeyValue}");
            }
        }
    }
}
