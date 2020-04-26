#define TRACE
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace WPFGame
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
            }
            else
            {
                GameEngine engine = new GameEngine();
                engine.StartRun();
            }
        }
    }
}
