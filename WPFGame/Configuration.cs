using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFGame
{
    public class Configuration
    {
        public ConfigResolution Resolution { get; set; }
        public int WindowStyle { get; set; }
        public int WindowState { get; set; }
        public Key Up { get; set; }
        public Key Down { get; set; }
        public Key Left { get; set; }
        public Key Right { get; set; }
        public Key Escape { get; set; }
        public Key Action { get; set; }
        public Key Action2 { get; set; }
        public Key Back { get; set; }
        public Key Space { get; set; }

        public Configuration()
        {
            Resolution = new ConfigResolution();
        }

        public Configuration(Configuration originalConfiguration)
        {
            Resolution = new ConfigResolution();
            Resolution.Width = originalConfiguration.Resolution.Width;
            Resolution.Height = originalConfiguration.Resolution.Height;
            WindowStyle = originalConfiguration.WindowStyle;
            WindowState = originalConfiguration.WindowState;
            Up = originalConfiguration.Up;
            Down = originalConfiguration.Down;
            Left = originalConfiguration.Left;
            Right = originalConfiguration.Right;
            Escape = originalConfiguration.Escape;
            Action = originalConfiguration.Action;
            Action2 = originalConfiguration.Action2;
            Back = originalConfiguration.Back;
            Space = originalConfiguration.Space;
        }
    }
}
