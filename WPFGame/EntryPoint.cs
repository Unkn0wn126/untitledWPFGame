#define TRACE
using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
                Game game = new Game(800, 600);
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"./Resources/Sounds/arena_loop2.wav");
                //player.Play();
                //player.SoundLocation = @"./Resources/Sounds/arena_loop2.wav";


                //RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
                MainWindow window = new MainWindow();
                CompositionTarget.Rendering += window.UpdateGraphics;

                var app = new App();

                app.Run(window);

            }
        }
    }
}
