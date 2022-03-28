using System;

namespace GameInputHandler
{
    /// <summary>
    /// Keeps track of all the
    /// registered virtual keys
    /// </summary>
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
        DetectiveMode = 1 << 7,
        Space = 1 << 8
    }

    /// <summary>
    /// Helper class
    /// to keep track
    /// of pressed keys
    /// </summary>
    public class GameInput
    {
        public GameKey CurrentKeyValue { get; set; }
    }
}
