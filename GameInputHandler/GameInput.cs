using System;

namespace GameInputHandler
{
    [Flags]
    public enum GameKey
    {
        None = 0,
        Escape = 1 << 0,
        Up = 1 << 1,
        Left = 1 << 2,
        Down = 1 << 3,
        Right = 1 << 4,
        Action = 1 << 5,
        Back = 1 << 6,
        Action2 = 1 << 7
    }
    public class GameInput
    {
        public GameKey CurrentKeyValue { get; set; }
    }
}
