using Engine.Models.MovementStateStrategies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WPFGame
{
    public class UserInputHandler
    {
        private readonly Dictionary<int, AxisStrategy> _userInputActions =
            new Dictionary<int, AxisStrategy>();

        private readonly Dictionary<Key, int> _keyCodes =
            new Dictionary<Key, int>();

        private List<Key> _previousKeys = new List<Key>();

        private int _currentKeyValue = 0;

        public UserInputHandler()
        {
            InitializeUserInputActions();
        }

        private void InitializeUserInputActions()
        {
            InitializeKeyCodes();
            InitializeKeyCodesMeaning();
        }

        private void InitializeKeyCodes()
        {
            // Kind of like a state machine of pressed down keys
            // powers of 2 to allow diagonal movement
            _keyCodes.Add(Key.W, 2);
            _keyCodes.Add(Key.A, 4);
            _keyCodes.Add(Key.S, 8);
            _keyCodes.Add(Key.D, 16);
        }

        private void InitializeKeyCodesMeaning()
        {
            _userInputActions.Add(2, AxisStrategy.UP);
            _userInputActions.Add(4, AxisStrategy.LEFT);
            _userInputActions.Add(6, AxisStrategy.UPLEFT);
            _userInputActions.Add(8, AxisStrategy.DOWN);
            _userInputActions.Add(12, AxisStrategy.DOWNLEFT);
            _userInputActions.Add(16, AxisStrategy.RIGHT);
            _userInputActions.Add(18, AxisStrategy.UPRIGHT);
            _userInputActions.Add(24, AxisStrategy.DOWNRIGHT);
        }

        public AxisStrategy HandleKeyPressed(Key e)
        {
            if (!_previousKeys.Contains(e))
            {
                _previousKeys.Add(e);
            }

            return ProcessPressedKeys(e);
        }

        public AxisStrategy HandleKeyReleased(Key e)
        {
            if (_previousKeys.Contains(e))
            {
                _previousKeys.Remove(e);
            }

            return ProcessPressedKeys(e);
        }

        private AxisStrategy ProcessPressedKeys(Key e)
        {
            _currentKeyValue = 0;

            foreach (var item in _previousKeys)
            {
                if (_keyCodes.ContainsKey(item))
                {
                    _currentKeyValue += _keyCodes[item];
                }
            }

            if (_userInputActions.ContainsKey(_currentKeyValue))
            {
                return _userInputActions[_currentKeyValue];
            }
            else
            {
                return AxisStrategy.NEUTRAL;
            }
        }
    }
}
