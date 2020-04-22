using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WPFGame
{
    public class Configuration
    {
        public int Width { get; set; }
        public int Height { get; set; }
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
    }
}
