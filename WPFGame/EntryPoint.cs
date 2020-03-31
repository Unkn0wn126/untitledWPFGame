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
                // ...
            }
            else
            {
                // sound testing
                //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"./Resources/Sounds/arena_loop2.wav");
                //player.Play();
                //player.SoundLocation = @"./Resources/Sounds/arena_loop2.wav";

                GameEngine engine = new GameEngine(800, 600);
                engine.StartRun();
            }
        }
    }
}
