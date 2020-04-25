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

        public void PerformDuplicateCheck()
        {
            List<Key> keys = new List<Key> { Up, Down, Left, Right, Escape, Action, Action2, Back, Space };
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                {
                    if(keys[i] == keys[j])
                    {
                        keys[j] = Key.None;
                    }
                }
            }

            Up = keys[0];
            Down = keys[1];
            Left = keys[2];
            Right = keys[3];
            Escape = keys[4];
            Action = keys[5];
            Action2 = keys[6];
            Back = keys[7];
            Space = keys[8];
        }
    }
}
