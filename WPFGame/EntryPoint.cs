using System;

namespace WPFGame
{
    /// <summary>
    /// Custom entry point
    /// to separate the logic
    /// from the graphics more
    /// </summary>
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            GameEngine engine = new GameEngine();
            engine.StartRun();
        }
    }
}
