using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFGame
{
    /// <summary>
    /// Class representing
    /// the game configuration
    /// </summary>
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
        public Key DetectiveMode { get; set; }
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
            DetectiveMode = originalConfiguration.DetectiveMode;
            Back = originalConfiguration.Back;
            Space = originalConfiguration.Space;
        }

        /// <summary>
        /// Removes all duplicate keys
        /// </summary>
        public void PerformDuplicateCheck()
        {
            List<Key> keys = new List<Key> { Up, Down, Left, Right, Escape, Action, DetectiveMode, Back, Space };
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
            DetectiveMode = keys[6];
            Back = keys[7];
            Space = keys[8];
        }
    }
}
