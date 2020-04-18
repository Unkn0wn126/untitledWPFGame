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
        //private readonly Dictionary<int, AxisStrategy> _userInputActions =
        //    new Dictionary<int, AxisStrategy>();

        private readonly Dictionary<Key, GameKey> _keyCodes =
            new Dictionary<Key, GameKey>();

        private List<Key> _previousKeys = new List<Key>();

        private int _currentKeyValue = 0;
        private GameInput _gameInputHandler;

        public UserInputHandler(GameInput gameInputHandler)
        {
            _gameInputHandler = gameInputHandler;
            InitializeUserInputActions();
        }

        private void InitializeUserInputActions()
        {
            InitializeKeyCodes();
            //InitializeKeyCodesMeaning();
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
        }

        //private void InitializeKeyCodesMeaning()
        //{
        //    _userInputActions.Add(2, AxisStrategy.UP);
        //    _userInputActions.Add(4, AxisStrategy.LEFT);
        //    _userInputActions.Add(6, AxisStrategy.UPLEFT);
        //    _userInputActions.Add(8, AxisStrategy.DOWN);
        //    _userInputActions.Add(12, AxisStrategy.DOWNLEFT);
        //    _userInputActions.Add(16, AxisStrategy.RIGHT);
        //    _userInputActions.Add(18, AxisStrategy.UPRIGHT);
        //    _userInputActions.Add(24, AxisStrategy.DOWNRIGHT);
        //}

        public void HandleKeyPressed(Key e)
        {
            //if (!_previousKeys.Contains(e))
            //{
            //_previousKeys.Add(e);
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue |= _keyCodes[e];
                Trace.WriteLine($"Pressing, value is: {_gameInputHandler.CurrentKeyValue}");
            }

            //}

            //ProcessPressedKeys(e);
        }

        public void HandleKeyReleased(Key e)
        {
            //if (_previousKeys.Contains(e))
            //{
            //_previousKeys.Remove(e);
            if (_keyCodes.ContainsKey(e))
            {
                _gameInputHandler.CurrentKeyValue &= ~_keyCodes[e];
                Trace.WriteLine($"Releasing, value is: {_gameInputHandler.CurrentKeyValue}");
            }

            //}

            //ProcessPressedKeys(e);
        }

        private void ProcessPressedKeys(Key e)
        {

            foreach (var item in _previousKeys)
            {
                if (_keyCodes.ContainsKey(item))
                {
                    //_gameInputHandler.CurrentKeyValue += _keyCodes[item];
                }
            }

            //if (_userInputActions.ContainsKey(_currentKeyValue))
            //{
            //    return _userInputActions[_currentKeyValue];
            //}
            //else
            //{
            //    return AxisStrategy.NEUTRAL;
            //}
        }
    }
}
